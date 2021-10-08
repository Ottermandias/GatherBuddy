using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Dalamud.Logging;
using GatherBuddy.Game;

namespace GatherBuddy.Classes
{
    public class FishRecord
    {
        public const uint MinTime = 1000;
        public const uint MaxTime = 45000;

        public HashSet<uint> SuccessfulBaits   { get; }      = new();
        public ushort        EarliestCatch     { get; set; } = ushort.MaxValue;
        public ushort        LatestCatch       { get; set; }
        public ushort        EarliestCatchChum { get; set; } = ushort.MaxValue;
        public ushort        LatestCatchChum   { get; set; }
        public bool          WithoutSnagging   { get; set; }

        public bool Update(Bait bait, ushort time, bool snagging, bool chum, long biteTime)
            => Update(bait, time, snagging, chum);

        public void Delete()
        {
            SuccessfulBaits.Clear();
            EarliestCatch     = ushort.MaxValue;
            EarliestCatchChum = ushort.MaxValue;
            LatestCatch       = 0;
            LatestCatchChum   = 0;
            WithoutSnagging   = false;
        }

        public bool Merge(FishRecord rhs)
        {
            var ret = false;
            if (rhs.SuccessfulBaits.Count == 0)
                return ret;

            if (rhs.EarliestCatch < EarliestCatch)
            {
                EarliestCatch = rhs.EarliestCatch;
                ret           = true;
            }

            if (rhs.EarliestCatchChum < EarliestCatchChum)
            {
                EarliestCatchChum = rhs.EarliestCatchChum;
                ret               = true;
            }

            if (rhs.LatestCatch > LatestCatch)
            {
                LatestCatch = rhs.LatestCatch;
                ret         = true;
            }

            if (rhs.LatestCatchChum > LatestCatchChum)
            {
                LatestCatchChum = rhs.LatestCatchChum;
                ret             = true;
            }


            if (rhs.WithoutSnagging && !WithoutSnagging)
            {
                WithoutSnagging = rhs.WithoutSnagging;
                ret             = true;
            }

            foreach (var bait in rhs.SuccessfulBaits)
                ret |= SuccessfulBaits.Add(bait);

            return ret;
        }

        public bool Update(Bait bait, ushort time, bool snagging, bool chum)
        {
            var ret = false;

            if (bait.Id != 0)
                ret |= SuccessfulBaits.Add(bait.Id);

            if (time > MinTime && time < MaxTime)
            {
                if (chum)
                {
                    if (time < EarliestCatchChum)
                    {
                        ret               = true;
                        EarliestCatchChum = time;
                    }

                    if (time > LatestCatchChum)
                    {
                        ret             = true;
                        LatestCatchChum = time;
                    }
                }
                else
                {
                    if (time < EarliestCatch)
                    {
                        ret           = true;
                        EarliestCatch = time;
                    }

                    if (time > LatestCatch)
                    {
                        ret         = true;
                        LatestCatch = time;
                    }
                }
            }

            if (snagging || WithoutSnagging)
                return ret;

            WithoutSnagging = true;
            ret             = true;

            return ret;
        }

        public string WriteLine(uint fishId)
        {
            var sb = new StringBuilder(128);
            sb.Append(fishId.ToString());
            sb.Append(" : { ");
            sb.Append(EarliestCatch.ToString());
            sb.Append(" ");
            sb.Append(LatestCatch.ToString());
            sb.Append(" ");
            sb.Append(EarliestCatchChum.ToString());
            sb.Append(" ");
            sb.Append(LatestCatchChum.ToString());
            sb.Append(WithoutSnagging ? " false [ " : " true [ ");
            foreach (var bait in SuccessfulBaits)
            {
                sb.Append(bait.ToString());
                sb.Append(" ");
            }

            sb.Append("]}");
            return sb.ToString();
        }

        private static readonly Regex V3MigrationRegex = new("(Unknown|Weak|Strong|Legendary) ", RegexOptions.Compiled);

        private static string MigrateToV3(string line)
            => V3MigrationRegex.Replace(line, "");

        public static (uint, FishRecord)? FromLine(string line)
        {
            (uint, FishRecord)? Error()
            {
                PluginLog.Error($"Could not create fishing record from \"{line}\".");
                return null;
            }

            line = MigrateToV3(line);
            var split  = line.Split(' ');
            var length = split.Length;

            const int fishIdOffset         = 0;
            const int colonOffset          = 1;
            const int openBraceOffset      = 2;
            var       earlyCatchOffset     = 3;
            var       lateCatchOffset      = 4;
            var       earlyCatchChumOffset = 5;
            var       lateCatchChumOffset  = 6;
            var       snaggingOffset       = 7;
            var       openBracketOffset    = 8;
            var       firstBaitOffset      = 9;
            var       closeBracketOffset   = length - 1;

            if (length < 8)
                return Error();

            if (length < 9 || split[openBracketOffset] != "[")
                if (split[openBracketOffset - 2] == "[")
                {
                    earlyCatchChumOffset =  -1;
                    lateCatchChumOffset  =  -1;
                    snaggingOffset       -= 2;
                    openBracketOffset    -= 2;
                    firstBaitOffset      -= 2;
                }

            if (split[colonOffset] != ":"
             || split[openBraceOffset] != "{"
             || split[openBracketOffset] != "["
             || split[closeBracketOffset] != "]}")
                return Error();

            if (!uint.TryParse(split[fishIdOffset], out var fishId))
                return Error();

            if (!ushort.TryParse(split[earlyCatchOffset], out var earliestCatch))
                return Error();

            if (!ushort.TryParse(split[lateCatchOffset], out var latestCatch))
                return Error();

            ushort earliestCatchChum;
            if (earlyCatchChumOffset == -1)
                earliestCatchChum = ushort.MaxValue;
            else if (!ushort.TryParse(split[earlyCatchChumOffset], out earliestCatchChum))
                return Error();

            ushort latestCatchChum;
            if (lateCatchChumOffset == -1)
                latestCatchChum = 0;
            else if (!ushort.TryParse(split[lateCatchChumOffset], out latestCatchChum))
                return Error();

            if (!bool.TryParse(split[snaggingOffset], out var snagging))
                return Error();


            FishRecord ret = new()
            {
                EarliestCatch     = earliestCatch,
                EarliestCatchChum = earliestCatchChum,
                LatestCatch       = latestCatch,
                LatestCatchChum   = latestCatchChum,
                WithoutSnagging   = !snagging,
            };
            for (var i = firstBaitOffset; i < closeBracketOffset; ++i)
            {
                if (!uint.TryParse(split[i], out var baitId))
                    return Error();

                ret.SuccessfulBaits.Add(baitId);
            }

            return (fishId, ret);
        }
    }
}

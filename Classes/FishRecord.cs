using System;
using System.Collections.Generic;
using System.Text;
using Dalamud.Plugin;
using GatherBuddy.Enums;
using GatherBuddy.Game;

namespace GatherBuddy.Classes
{
    public class FishRecord
    {
        public HashSet<uint> SuccessfulBaits   { get; }      = new();
        public ushort        EarliestCatch     { get; set; } = ushort.MaxValue;
        public ushort        LatestCatch       { get; set; }
        public ushort        EarliestCatchChum { get; set; } = ushort.MaxValue;
        public ushort        LatestCatchChum   { get; set; }
        public bool          WithoutSnagging   { get; set; }
        public BiteType      BiteType          { get; set; }

        public bool Update(Bait bait, ushort time, bool snagging, bool chum, long biteTime)
            => Update(bait, time, snagging, chum, BiteTypeExtension.FromBiteTime(biteTime));

        public void Delete()
        {
            SuccessfulBaits.Clear();
            EarliestCatch     = ushort.MaxValue;
            EarliestCatchChum = ushort.MaxValue;
            LatestCatch       = 0;
            LatestCatchChum   = 0;
            WithoutSnagging   = false;
            BiteType          = BiteType.Unknown;
        }

        public bool Update(Bait bait, ushort time, bool snagging, bool chum, BiteType bite)
        {
            const uint minTime = 1000;
            const uint maxTime = 40000;
            var        ret     = false;
            if (BiteType == BiteType.Unknown)
            {
                BiteType = bite;
                ret      = bite != BiteType.Unknown;
            }

            if (bite != BiteType)
            {
                PluginLog.Error($"Invalid bite type for record, previously {BiteType} and now {bite}.");
                return false;
            }

            ret |= SuccessfulBaits.Add(bait.Id);
            if (time > minTime && time < maxTime)
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
            sb.Append(BiteType.ToString());
            sb.Append(" ");
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

        public static (uint, FishRecord)? FromLine(string line)
        {
            (uint, FishRecord)? Error()
            {
                PluginLog.Error($"Could not create fishing record from \"{line}\".");
                return null;
            }

            var split  = line.Split(' ');
            var length = split.Length;

            const int fishIdOffset         = 0;
            const int colonOffset          = 1;
            const int openBraceOffset      = 2;
            const int biteOffset           = 3;
            const int earlyCatchOffset     = 4;
            const int lateCatchOffset      = 5;
            var       earlyCatchChumOffset = 6;
            var       lateCatchChumOffset  = 7;
            var       snaggingOffset       = 8;
            var       openBracketOffset    = 9;
            var       firstBaitOffset      = 10;
            var       closeBracketOffset   = length - 1;

            if (length < 9)
                return Error();

            if (length < 10 || split[openBracketOffset] != "[")
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

            if (!Enum.TryParse<BiteType>(split[biteOffset], out var biteType))
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
                BiteType          = biteType,
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

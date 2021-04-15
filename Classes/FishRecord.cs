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
        public HashSet<uint> SuccessfulBaits { get; }      = new();
        public ushort        EarliestCatch   { get; set; } = ushort.MaxValue;
        public ushort        LatestCatch     { get; set; }
        public bool          WithoutSnagging { get; set; }
        public BiteType      BiteType        { get; set; }

        public bool Update(Bait bait, ushort time, bool snagging, long biteTime)
            => Update(bait, time, snagging, BiteTypeExtension.FromBiteTime(biteTime));

        public void Delete()
        {
            SuccessfulBaits.Clear();
            EarliestCatch   = ushort.MaxValue;
            LatestCatch     = 0;
            WithoutSnagging = false;
            BiteType        = BiteType.Unknown;
        }

        public bool Update(Bait bait, ushort time, bool snagging, BiteType bite)
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
            if (length < 9 || split[1] != ":" || split[2] != "{" || split[7] != "[" || split[length - 1] != "]}")
                return Error();

            if (!uint.TryParse(split[0], out var fishId))
                return Error();

            if (!Enum.TryParse<BiteType>(split[3], out var biteType))
                return Error();

            if (!ushort.TryParse(split[4], out var earliestCatch))
                return Error();

            if (!ushort.TryParse(split[5], out var latestCatch))
                return Error();

            if (!bool.TryParse(split[6], out var snagging))
                return Error();

            FishRecord ret = new()
            {
                BiteType        = biteType,
                EarliestCatch   = earliestCatch,
                LatestCatch     = latestCatch,
                WithoutSnagging = !snagging,
            };
            for (var i = 8; i < length - 1; ++i)
            {
                if (!uint.TryParse(split[i], out var baitId))
                    return Error();

                ret.SuccessfulBaits.Add(baitId);
            }

            return (fishId, ret);
        }
    }
}

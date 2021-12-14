using System.Collections.Generic;

namespace GatherBuddy.FishTimer;

public class FishRecordTimes
{
    public struct Times
    {
        public ushort Min     = ushort.MaxValue;
        public ushort Max     = 0;
        public ushort MinChum = ushort.MaxValue;
        public ushort MaxChum = 0;

        public Times()
        { }

        public bool Apply(ushort duration, bool chum)
        {
            var ret = false;
            if (chum)
            {
                if (duration > MaxChum)
                {
                    MaxChum = duration;
                    ret     = true;
                }
            }
            else
            {
                if (duration < Min)
                {
                    Min = duration;
                    ret = true;
                }
            }

            if (duration > Max)
            {
                Max = duration;
                ret = true;
            }

            if (duration < MinChum)
            {
                MinChum = duration;
                ret     = true;
            }

            return ret;
        }
    }

    public Times                   All  = new();
    public SortedList<uint, Times> Data = new();

    public bool Apply(uint baitId, ushort duration, bool chum)
    {
        var ret = All.Apply(duration, chum);
        if (baitId == 0)
            return ret;

        var baitTimes = Data.TryGetValue(baitId, out var b) ? b : new Times();
        ret          |= baitTimes.Apply(duration, chum);
        Data[baitId] =  baitTimes;

        return ret;
    }
}

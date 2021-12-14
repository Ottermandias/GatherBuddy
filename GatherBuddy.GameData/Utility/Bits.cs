namespace GatherBuddy.Utility;

public static class Bits
{
    public static uint Popcount(uint value)
    {
        value -= (value >> 1) & 0x55555555u;
        value =  (value & 0x33333333u) + ((value >> 2) & 0x33333333u);
        return (((value + (value >> 4)) & 0xF0F0F0Fu) * 0x1010101u) >> 24;
    }

    public static uint HighestSetBit(uint value)
    {
        var r = 0u; // result of log2(v) will go here
        if ((value & 0xFFFF0000u) != 0u)
        {
            value >>= 16;
            r     |=  16;
        }

        if ((value & 0xFF00u) != 0u)
        {
            value >>= 8;
            r     |=  8;
        }

        if ((value & 0xF0u) != 0u)
        {
            value >>= 4;
            r     |=  4;
        }

        if ((value & 0xCu) != 0u)
        {
            value >>= 2;
            r     |=  2;
        }

        return (value & 0x2) != 0u ? r | 1 : r;
    }

    public static uint TrailingZeroCount(uint value)
    {
        if ((value & 0x1u) == 0x1u)
            return 0;

        var ret = 1;
        if ((value & 0xFFFFu) == 0)
        {
            value >>= 16;
            ret   +=  16;
        }

        if ((value & 0xFFu) == 0)
        {
            value >>= 8;
            ret   +=  8;
        }

        if ((value & 0xFu) == 0)
        {
            value >>= 4;
            ret   +=  4;
        }

        if ((value & 0x3u) == 0)
        {
            value >>= 2;
            ret   +=  2;
        }

        return (uint)(ret - (value & 0x1));
    }
}

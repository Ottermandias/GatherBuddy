using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GatherBuddy.FishTimer.OldRecords;

public struct Record
{
    public const uint MinTime = 1000;
    public const uint MaxTime = 45000;

    public HashSet<uint> SuccessfulBaits   { get; }      = new();
    public ushort        EarliestCatch     { get; set; } = ushort.MaxValue;
    public ushort        LatestCatch       { get; set; } = 0;
    public ushort        EarliestCatchChum { get; set; } = ushort.MaxValue;
    public ushort        LatestCatchChum   { get; set; } = 0;
    public bool          WithoutSnagging   { get; set; } = false;

    public Record()
    {}

    private static readonly Regex V3MigrationRegex = new("(Unknown|Weak|Strong|Legendary) ", RegexOptions.Compiled);

    private static string MigrateToV3(string line)
        => V3MigrationRegex.Replace(line, "");

    public static (uint, Record)? FromLine(string line)
    {
        (uint, Record)? Error()
        {
            GatherBuddy.Log.Error($"Could not create fishing record from \"{line}\".");
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


        Record ret = new()
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

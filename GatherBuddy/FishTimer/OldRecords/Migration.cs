using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GatherBuddy.Enums;
using GatherBuddy.Plugin;
using GatherBuddy.Structs;

namespace GatherBuddy.FishTimer.OldRecords;

public static class Migration
{
    public const string SaveFileName = "fishing_records.data";

    private static void AddOldRecord(uint fishId, Record oldRecord, ICollection<FishRecord> records)
    {
        if (oldRecord.SuccessfulBaits.Count == 0)
            return;

        if (!GatherBuddy.GameData.Fishes.TryGetValue(fishId, out var fish))
            return;

        var bait = oldRecord.SuccessfulBaits.Count > 1
            ? Bait.Unknown
            : GatherBuddy.GameData.Bait.TryGetValue(oldRecord.SuccessfulBaits.First(), out var b)
                ? b
                : GatherBuddy.GameData.Fishes.TryGetValue(oldRecord.SuccessfulBaits.First(), out var f)
                    ? new Bait(f.ItemData)
                    : Bait.Unknown;

        var flags = FishRecord.Effects.Valid | FishRecord.Effects.Legacy;
        if (!oldRecord.WithoutSnagging)
            flags |= FishRecord.Effects.Snagging;
        if (fish.Predators.Length > 0)
            flags |= FishRecord.Effects.Intuition;

        var record = new FishRecord
        {
            Amount        = 1,
            TimeStamp     = GatherBuddy.Time.ServerTime,
            Bait          = bait,
            Catch         = fish,
            ContentIdHash = 0,
            FishingSpot   = null,
            Gathering     = 0,
            Perception    = 0,
            Size          = 0,
            Flags         = flags,
        };
        record.SetTugHook(fish.BiteType, HookSet.Hook);

        if (oldRecord.EarliestCatch < Record.MaxTime && oldRecord.EarliestCatch > Record.MinTime)
            records.Add(record with { Bite = oldRecord.EarliestCatch });

        if (oldRecord.LatestCatch < Record.MaxTime && oldRecord.LatestCatch > oldRecord.EarliestCatch)
            records.Add(record with { Bite = oldRecord.LatestCatch });

        record.Flags |= FishRecord.Effects.Chum;

        if (oldRecord.EarliestCatchChum < Record.MaxTime && oldRecord.EarliestCatchChum > Record.MinTime)
            records.Add(record with { Bite = oldRecord.EarliestCatchChum });

        if (oldRecord.LatestCatchChum < Record.MaxTime && oldRecord.LatestCatchChum > oldRecord.EarliestCatchChum)
            records.Add(record with { Bite = oldRecord.LatestCatchChum });
    }

    public static void MigrateRecords(FishRecorder recorder)
    {
        var file = Functions.ObtainSaveFile(SaveFileName);
        if (file is not { Exists: true })
            return;

        try
        {
            var text       = File.ReadAllLines(file.FullName);
            file.MoveTo(file.FullName + ".backup", true);
            var newRecords = new List<FishRecord>(4 * text.Length);
            foreach (var line in text)
            {
                var (fishId, oldRecord) = Record.FromLine(line) ?? (0, default);
                if (fishId != 0)
                    AddOldRecord(fishId, oldRecord, newRecords);
            }
            recorder.MergeRecordsIn(newRecords);
            file.Delete();
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Could not read old fishing records from file {file.FullName}:\n{e}");
        }
    }
}

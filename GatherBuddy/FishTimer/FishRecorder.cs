using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Dalamud.Plugin.Services;
using GatherBuddy.FishTimer.Parser;
using GatherBuddy.Models;

namespace GatherBuddy.FishTimer;

public partial class FishRecorder : IDisposable
{
    public readonly List<FishRecord>                  Records = new();
    public readonly Dictionary<uint, FishRecordTimes> Times   = new();

    public FishRecorder(IGameInteropProvider provider)
    {
        FishRecordDirectory = Dalamud.PluginInterface.ConfigDirectory;
        try
        {
            Directory.CreateDirectory(FishRecordDirectory.FullName);
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Could not create fish record directory {FishRecordDirectory.FullName}:\n{e}");
        }

        var file = new FileInfo(Path.Combine(FishRecordDirectory.FullName, FishRecordFileName));
        LoadFile(file);

        LoadRemoteFile();

        if (Changes > 0)
        {
            WriteFile();
            ResetTimes();
        }

        Parser = new FishingParser(provider);
        SubscribeToParser();
        UsedLure += LureTimer.Restart;
    }

    public void Enable()
    {
        Parser.Enable();
        Dalamud.Framework.Update += OnFrameworkUpdate;
    }

    public void Disable()
    {
        Dalamud.Framework.Update -= OnFrameworkUpdate;
        Parser.Disable();
    }

    public void Dispose()
    {
        Disable();
        Parser.Dispose();
        StopRemoteRecordsRequests();
        WriteRemoteFile();
        if (Changes > 0)
            WriteFile();
    }

    private void AddUnchecked(FishRecord record)
    {
        Records.Add(record);
        RecordsToUpload.Enqueue(record);
        AddRecordToTimes(record);
        AddChanges();
    }

    internal bool AddChecked(FishRecord record)
    {
        if (!CheckSimilarity(record))
            return false;

        AddUnchecked(record);
        return true;
    }

    private void AddRecordToTimes(FishRecord record)
    {
        if (record.Catch == null || !record.Flags.HasFlag(Effects.Valid) || record.Flags.HasLure() && !record.Flags.HasValidLure())
            return;

        if (!Times.TryGetValue(record.Catch.ItemId, out var times))
        {
            times                      = new FishRecordTimes();
            Times[record.Catch.ItemId] = times;
        }

        times.Apply(record.Bait.Id, record.Bite, record.Flags.HasFlag(Effects.Chum));
    }

    public void Add(FishRecord record)
    {
        AddChecked(record);
        if (Changes > 20)
            WriteFile();
    }

    public void Remove(int idx)
    {
        Debug.Assert(idx >= 0 && idx < Records.Count);
        var record = Records[idx];
        Records.RemoveAt(idx);
        RemoveRecordFromTimes(record);
        if (AddChanges() > 20)
            WriteFile();
    }

    private void RemoveRecordFromTimes(FishRecord record)
    {
        if (!record.Flags.HasFlag(Effects.Valid) || !record.HasCatch)
            return;

        if (!Times.TryGetValue(record.Catch!.ItemId, out var data) || !data.Data.TryGetValue(record.Bait.Id, out var times))
        {
            GatherBuddy.Log.Error("Invalid state in fish records.");
            return;
        }

        if (times.Max != record.Bite && times.MaxChum != record.Bite && times.Min != record.Bite && times.MinChum != record.Bite)
            return;

        data.Data.Remove(record.Bait.Id);
        foreach (var rec in Records.Where(r
                     => r.Flags.HasFlag(Effects.Valid)
                  && (!record.Flags.HasLure() || record.Flags.HasValidLure())
                  && r.Catch?.ItemId == record.Catch.ItemId
                  && r.Bait.Id == record.Bait.Id))
            data.Apply(rec.Bait.Id, rec.Bite, rec.Flags.HasFlag(Effects.Chum));

        if (data.Data.Count != 0)
        {
            if (data.All.Max == record.Bite)
                data.All.Max = data.Data.Values.Max(r => r.Max);
            if (data.All.MaxChum == record.Bite)
                data.All.MaxChum = data.Data.Values.Max(r => r.MaxChum);
            if (data.All.Min == record.Bite)
                data.All.Min = data.Data.Values.Min(r => r.Min);
            if (data.All.MinChum == record.Bite)
                data.All.MinChum = data.Data.Values.Min(r => r.MinChum);
        }
        else
        {
            data.All = default;
        }
    }

    private void ResetTimes()
    {
        Times.Clear();
        foreach (var record in Records)
            AddRecordToTimes(record);
    }

    public void RemoveInvalid()
    {
        if (Records.RemoveAll(r => !r.Flags.HasFlag(Effects.Valid)) <= 0)
            return;

        ResetTimes();
        WriteFile();
    }

    public void RemoveDuplicates()
    {
        var oldCount = Records.Count;
        for (var i = 0; i < Records.Count; ++i)
        {
            var rec = Records[i];
            for (var j = Records.Count - 1; j > i; --j)
            {
                if (Similar(rec, Records[j]))
                    Records.RemoveAt(j);
            }
        }

        if (oldCount == Records.Count)
            return;

        ResetTimes();
        WriteFile();
    }

    private static bool Similar(FishRecord lhs, FishRecord rhs)
        => lhs.Flags.HasFlag(Effects.Legacy)
            ? lhs.Flags == rhs.Flags && lhs.CatchId == rhs.CatchId && lhs.Bite == rhs.Bite
            : Math.Abs(lhs.TimeStamp - rhs.TimeStamp) < 1000;

    private bool CheckSimilarity(FishRecord record)
        => !Records.Any(r => Similar(r, record));
}

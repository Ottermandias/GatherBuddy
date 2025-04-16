using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GatherBuddy.Plugin;
using GatherBuddy.Time;
using MessagePack;
using Newtonsoft.Json;

namespace GatherBuddy.FishTimer;

public partial class FishRecorder
{
    public const    string        FishRecordFileName = "fish_records.dat";
    public readonly DirectoryInfo FishRecordDirectory;

    public  int       Changes  = 0;
    public  TimeStamp SaveTime = TimeStamp.MaxValue;

    public int AddChanges()
    {
        SaveTime = TimeStamp.UtcNow.AddMinutes(3);
        return ++Changes;
    }

    public void WriteFile()
    {
        var file = new FileInfo(Path.Combine(FishRecordDirectory.FullName, FishRecordFileName));
        Changes  = 0;
        SaveTime = TimeStamp.MaxValue;
        WriteFileInternal(file, false);
    }

    private void WriteFileInternal(FileInfo file, bool remote)
    {
        GatherBuddy.Log.Debug($"Saving fish record file to {file.FullName} with {Changes} changes.");
        try
        {
            var bytes = GetRecordBytes(remote);
            File.WriteAllBytes(file.FullName, bytes);
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Could not write fish record file {file.FullName}:\n{e}");
        }
    }

    private void TimedSave()
    {
        if (TimeStamp.UtcNow > SaveTime)
        {
            WriteFile();
        }
    }

    public string ExportBase64()
    {
        var bytes = GetRecordBytes(false);
        return Functions.CompressedBase64(bytes);
    }

    public void ExportJson(FileInfo file)
    {
        try
        {
            var data = JsonConvert.SerializeObject(Records.Select(r => r.ToJson()), Formatting.Indented);
            File.WriteAllText(file.FullName, data);
            GatherBuddy.Log.Information($"Exported {Records.Count} fish records to {file.FullName}.");
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Warning($"Could not export json file to {file.FullName}:\n{e}");
        }
    }

    public void ImportBase64(string data)
    {
        try
        {
            var bytes   = Functions.DecompressedBase64(data);
            var records = ReadBytes(bytes, "Imported Data");
            MergeRecordsIn(records);
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Warning($"Error while importing fish records:\n{e}");
        }
    }

    public void MergeRecordsIn(IReadOnlyList<FishRecord> newRecords)
    {
        foreach (var record in newRecords.Where(CheckSimilarity))
            AddUnchecked(record);

        if (Changes > 0)
            WriteFile();
    }

    public static List<FishRecord> ReadFile(FileInfo file)
    {
        if (!file.Exists)
            return new List<FishRecord>();

        try
        {
            var bytes = File.ReadAllBytes(file.FullName);
            return ReadBytes(bytes, $"File {file.FullName}");
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Unknown error reading fish record file {file.FullName}:\n{e}");
            return new List<FishRecord>();
        }
    }

    private byte[] GetRecordBytes(bool remote)
    {
        using var ms = new MemoryStream();
        ms.WriteByte(FishRecord.Version);

        var records = remote ? RemoteRecords : Records;
        MessagePackSerializer.Serialize(ms, records);

        return ms.ToArray();
    }

    private static List<FishRecord> ReadBytes(byte[] data, string name)
    {
        if (data.Length == 0)
            return new List<FishRecord>();

        switch (data[0])
        {
            case 1:
            {
                if (data.Length % FishRecord.Version1ByteLength != 1)
                {
                    GatherBuddy.Log.Error($"{name} has no valid size for its record version, skipped.\n");
                    return new List<FishRecord>();
                }

                var numRecords = (data.Length - 1) / FishRecord.Version1ByteLength;
                var ret        = new List<FishRecord>(numRecords);
                for (var i = 0; i < numRecords; ++i)
                {
                    if (!FishRecord.FromBytesV1(data, 1 + i * FishRecord.Version1ByteLength, out var record))
                    {
                        GatherBuddy.Log.Error($"{name}'s {i}th record is invalid, skipped.\n");
                        continue;
                    }

                    ret.Add(record);
                }

                return ret;
            }
            case 2:
            {
                var span = data.AsSpan()[1..];
                try
                {
                    var list = MessagePackSerializer.Deserialize<List<FishRecord>>(span.ToArray());
                    return list;
                }
                catch (Exception e)
                {
                    GatherBuddy.Log.Error($"{name} was unable to be deserialized using V2 logic.");
                    return new List<FishRecord>();
                }
            }
            default:
                GatherBuddy.Log.Error($"{name} has no valid record version, skipped.\n");
                return new List<FishRecord>();
        }
    }

    private void LoadFile(FileInfo file)
    {
        if (!file.Exists)
            return;

        try
        {
            Records.AddRange(ReadFile(file));
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Could not read fish record file {file.FullName}:\n{e}");
        }
        ResetTimes();
    }
}

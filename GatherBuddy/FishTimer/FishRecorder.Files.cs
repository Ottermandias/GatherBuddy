using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GatherBuddy.Plugin;
using Newtonsoft.Json;

namespace GatherBuddy.FishTimer;

public partial class FishRecorder
{
    public const    string        FishRecordFileName = "fish_records.dat";
    public readonly DirectoryInfo FishRecordDirectory;

    public int Changes = 0;

    public void WriteFile()
    {
        var file = new FileInfo(Path.Combine(FishRecordDirectory.FullName, FishRecordFileName));
        try
        {
            var bytes = GetRecordBytes();
            File.WriteAllBytes(file.FullName, bytes);
            Changes = 0;
        }
        catch (Exception e)
        {
            GatherBuddy.Log.Error($"Could not write fish record file {file.FullName}:\n{e}");
        }
    }

    public string ExportBase64()
    {
        var bytes = GetRecordBytes();
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

    private byte[] GetRecordBytes()
    {
        var bytes = new byte[Records.Count * FishRecord.ByteLength + 1];
        bytes[0] = FishRecord.Version;
        for (var i = 0; i < Records.Count; ++i)
        {
            var record = Records[i];
            var offset = 1 + i * FishRecord.ByteLength;
            record.ToBytes(bytes, offset);
        }

        return bytes;
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
            default:
                GatherBuddy.Log.Error($"{name} has no valid record version, skipped.\n");
                return new List<FishRecord>();
        }
    }

    private void LoadFile()
    {
        var file = new FileInfo(Path.Combine(FishRecordDirectory.FullName, FishRecordFileName));
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

    private void MigrateOldFiles()
    {
        foreach (var file in FishRecordDirectory.EnumerateFiles("fish_records_*.dat"))
        {
            try
            {
                Records.AddRange(ReadFile(file));
                file.Delete();
                ++Changes;
            }
            catch (Exception e)
            {
                GatherBuddy.Log.Error($"Error migrating fish record file {file.FullName}:\n{e}");
            }
        }

        OldRecords.Migration.MigrateRecords(this);
    }
}

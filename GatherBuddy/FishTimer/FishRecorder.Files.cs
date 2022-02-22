using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Dalamud.Logging;
using GatherBuddy.Plugin;
using Newtonsoft.Json;

namespace GatherBuddy.FishTimer;

public partial class FishRecorder
{
    public const string        FishRecordFileFormat = "fish_records_{0:D6}.dat";
    public       DirectoryInfo FishRecordDirectory;

    public const int RecordsPerFile = 100;

    public void WriteFile(FileInfo file, int from, int amount)
    {
        Debug.Assert(from >= 0);
        amount = Math.Min(amount, Records.Count - from);
        
        try
        {
            if (amount == 0)
            {
                file.Delete();
            }
            else
            {
                var bytes = GetRecordBytes(from, amount);
                File.WriteAllBytes(file.FullName, bytes);
            }
        }
        catch (Exception e)
        {
            PluginLog.Error($"Could not write fish record file {file.FullName}:\n{e}");
        }
    }

    public void WriteAllFiles(int idx = 0)
    {
        var start    = idx / RecordsPerFile;
        var numFiles = Records.Count / RecordsPerFile + 1;
        for (var i = start; i < numFiles; ++i)
        {
            var name = string.Format(FishRecordFileFormat, i);
            var file = new FileInfo(Path.Combine(FishRecordDirectory.FullName, name));
            WriteFile(file, i * RecordsPerFile, RecordsPerFile);
        }
    }


    public void WriteFullFile(FileInfo file)
        => WriteFile(file, 0, Records.Count);

    public string ExportBase64(int from, int amount)
    {
        amount = Math.Min(amount, Records.Count - from);
        var bytes = GetRecordBytes(from, amount);
        return Functions.CompressedBase64(bytes);
    }

    public void ExportJson(FileInfo file)
    {
        try
        {
            var data = JsonConvert.SerializeObject(Records.Select(r => r.ToJson()), Formatting.Indented);
            File.WriteAllText(file.FullName, data);
            PluginLog.Information($"Exported {Records.Count} fish records to {file.FullName}.");
        }
        catch (Exception e)
        {
            PluginLog.Warning($"Could not export json file to {file.FullName}:\n{e}");
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
            PluginLog.Warning($"Error while importing fish records:\n{e}");
        }
    }

    public void WriteNewestFile()
    {
        if (Records.Count == 0)
            return;

        var newestFile = Records.Count / RecordsPerFile;
        var name       = string.Format(FishRecordFileFormat, newestFile);
        var file       = new FileInfo(Path.Combine(FishRecordDirectory.FullName, name));
        WriteFile(file, newestFile * RecordsPerFile, RecordsPerFile);
    }

    public void MergeRecordsIn(IReadOnlyList<FishRecord> newRecords)
    {
        foreach (var record in newRecords.Where(CheckSimilarity))
        {
            AddUnchecked(record);
            if (Records.Count % RecordsPerFile == RecordsPerFile - 1)
                WriteNewestFile();
        }

        if (Records.Count % RecordsPerFile != RecordsPerFile - 1)
            WriteNewestFile();
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
            PluginLog.Error($"Unknown error reading fish record file {file.FullName}:\n{e}");
            return new List<FishRecord>();
        }
    }

    private byte[] GetRecordBytes(int from, int amount)
    {
        var bytes = new byte[amount * FishRecord.ByteLength + 1];
        bytes[0] = FishRecord.Version;
        for (var i = 0; i < amount; ++i)
        {
            var record = Records[from + i];
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
                    PluginLog.Error($"{name} has no valid size for its record version, skipped.\n");
                    return new List<FishRecord>();
                }

                var numRecords = (data.Length - 1) / FishRecord.Version1ByteLength;
                var ret        = new List<FishRecord>(numRecords);
                for (var i = 0; i < numRecords; ++i)
                {
                    if (!FishRecord.FromBytesV1(data, 1 + i * FishRecord.Version1ByteLength, out var record))
                    {
                        PluginLog.Error($"{name}'s {i}th record is invalid, skipped.\n");
                        continue;
                    }

                    ret.Add(record);
                }

                return ret;
            }
            default:
                PluginLog.Error($"{name} has no valid record version, skipped.\n");
                return new List<FishRecord>();
        }
    }

    private void ReadAllFiles()
    {
        foreach (var file in FishRecordDirectory.EnumerateFiles("fish_records_*.dat"))
            Records.AddRange(ReadFile(file));
        OldRecords.Migration.MigrateRecords(this);
        ResetTimes();
    }
}

using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using GatherBuddy.AutoHookIntegration.Models;
using Newtonsoft.Json;

namespace GatherBuddy.AutoHookIntegration;

public static class AutoHookExporter
{
    private const string ExportPrefix = "AH4_";

    public static string ExportPreset(AHCustomPresetConfig preset)
    {
        var json = JsonConvert.SerializeObject(preset, 
            new JsonSerializerSettings 
            { 
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });
        
        var compressed = CompressString(json);
        return ExportPrefix + compressed;
    }

    private static string CompressString(string s)
    {
        var bytes = Encoding.UTF8.GetBytes(s);
        using var ms = new MemoryStream();
        using (var gs = new GZipStream(ms, CompressionMode.Compress))
        {
            gs.Write(bytes, 0, bytes.Length);
        }
        return Convert.ToBase64String(ms.ToArray());
    }
}

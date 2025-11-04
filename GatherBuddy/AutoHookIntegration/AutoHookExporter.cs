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
                DefaultValueHandling = DefaultValueHandling.Include,
                NullValueHandling = NullValueHandling.Ignore
            });
        
        // Log first 2000 chars to see hookset config
        var logLength = Math.Min(2000, json.Length);
        GatherBuddy.Log.Debug($"[AutoHook Export] JSON (first {logLength} chars):\n{json.Substring(0, logLength)}");
        
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

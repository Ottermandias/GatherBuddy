using System.IO.Compression;
using GatherBuddy.Web.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace GatherBuddy.Web.Controllers;

public class ClientAccessControlFilter : IAsyncActionFilter
{
    private const    string HashHeaderName = "X-Client-Hash";
    private const    string VersionHeaderName = "X-Client-Version";
    private const    string releasesUrl = "https://github.com/FFXIV-CombatReborn/GatherBuddyReborn/releases/download";
    private readonly ILogger<ClientAccessControlFilter> _logger;
    private readonly IMemoryCache _memoryCache;

    public ClientAccessControlFilter(ILogger<ClientAccessControlFilter> logger, IMemoryCache memoryCache)
    {
        _logger      = logger;
        _memoryCache = memoryCache;
    }

    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
#if DEBUG
        _logger.LogInformation("Skipping client access control check in debug mode.");
        return next();
#endif
        if (!context.HttpContext.Request.Headers.TryGetValue(VersionHeaderName, out var extractedVersion))
        {
            _logger.LogWarning($"No Version provided from {context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
             ?? context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"}.");
            context.Result = new UnauthorizedResult();
            return Task.CompletedTask;
        }

        if (!context.HttpContext.Request.Headers.TryGetValue(HashHeaderName, out var extractedHash))
        {
            _logger.LogWarning($"No Hash provided from {context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
             ?? context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"}.");
            context.Result = new UnauthorizedResult();
            return Task.CompletedTask;
        }

        var version = extractedVersion.FirstOrDefault();
        var hash    = extractedHash.FirstOrDefault();

        if (version == null || hash == null)
        {
            _logger.LogWarning($"Invalid Version or Hash provided from {context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
             ?? context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"}.");
            context.Result = new UnauthorizedResult();
            return Task.CompletedTask;
        }

        if (!_memoryCache.TryGetValue(version, out string? cachedHash))
        {
            Stream dllStream;
            using (var client = new HttpClient())
            {
                _logger.LogInformation($"Downloading {version} from {releasesUrl}");
                var response = client.GetAsync($"{releasesUrl}/{version}/GatherBuddyReborn.zip").Result;
                var content  = response.Content.ReadAsStreamAsync().Result;
                var zip      = new ZipArchive(content, ZipArchiveMode.Read);
                var hashFile = zip.GetEntry("GatherBuddyReborn.dll");
                if (hashFile == null)
                    throw new Exception("Hash file not found in zip archive.");

                dllStream = hashFile.Open();
            }

            using var sha256     = System.Security.Cryptography.SHA256.Create();
            var       hashBytes  = sha256.ComputeHash(dllStream);
            var       hashString = Convert.ToHexString(hashBytes);
            cachedHash           = hashString;
            _memoryCache.Set(version, cachedHash, TimeSpan.FromDays(5));
        }

        if (cachedHash == hash)
            return next();

        _logger.LogWarning($"Hash mismatch for {version} from {context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
         ?? context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"}.");
        context.Result = new UnauthorizedResult();
        return Task.CompletedTask;
    }
}

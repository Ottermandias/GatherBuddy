using GatherBuddy.Web.Database;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;

namespace GatherBuddy.Web.Controllers;

public class RateLimitFilter : IAsyncActionFilter
{
    private const string RateLimitHeaderName = "X-Rate-Limit";
    private const string RateLimitRemainingHeaderName = "X-Rate-Limit-Remaining";
    private const string RateLimitResetHeaderName = "X-Rate-Limit-Reset";
    private const int    RateLimit = 100;
    private const int    RateLimitReset = 60;

    private readonly IMemoryCache _cache;
    private readonly ILogger<RateLimitFilter> _logger;

    public RateLimitFilter(IMemoryCache memoryCache, ILogger<RateLimitFilter> logger)
    {
        _logger = logger;
        _cache = memoryCache;
    }
    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var ipAddress = context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault() ?? context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var key       = $"{ipAddress}:{context.HttpContext.Request.Path}";
        var remaining = _cache.GetOrCreate(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(RateLimitReset);
            return RateLimit;
        });

        remaining--;
        if (remaining < 0)
        {
            _logger.LogWarning($"Rate limit exceeded for {ipAddress}.");
            context.HttpContext.Response.Headers.Append(RateLimitHeaderName, "true");
            context.HttpContext.Response.Headers.Append(RateLimitRemainingHeaderName, "0");
            context.HttpContext.Response.Headers.Append(RateLimitResetHeaderName, RateLimitReset.ToString());
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            return Task.CompletedTask;
        }
        else
        {
            _logger.LogInformation($"Rate limit not exceeded for {ipAddress}. Rate limit: {remaining}.");
            context.HttpContext.Response.Headers.Append(RateLimitHeaderName, "false");
            context.HttpContext.Response.Headers.Append(RateLimitRemainingHeaderName, remaining.ToString());
            return next();
        }
    }
}

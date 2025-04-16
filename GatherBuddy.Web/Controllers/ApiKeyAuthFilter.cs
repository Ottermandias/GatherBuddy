using GatherBuddy.Web.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GatherBuddy.Web.Controllers;

public class ApiKeyAuthFilter : IAsyncActionFilter
{
    private const    string                    ApiKeyHeaderName = "X-API-Key";
    private readonly GatherBuddyDbContext      _dbContext;
    private readonly ILogger<ApiKeyAuthFilter> _logger;

    public ApiKeyAuthFilter(ILogger<ApiKeyAuthFilter> logger, GatherBuddyDbContext dbContext)
    {
        _logger    = logger;
        _dbContext = dbContext;
    }

    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedKey))
        {
            _logger.LogWarning($"No API key provided from {context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
             ?? context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"}.");
            context.Result = new UnauthorizedResult();
            return Task.CompletedTask;
        }

        var apiKey = extractedKey.ToString();

        var keyRecord = _dbContext.SecretKeys.FirstOrDefault(k => k.Key == apiKey);

        if (keyRecord == null)
        {
            _logger.LogWarning($"Invalid API key provided from {context.HttpContext.Connection.RemoteIpAddress}.");
            context.Result = new UnauthorizedResult();
            return Task.CompletedTask;
        }

        return next();
    }
}

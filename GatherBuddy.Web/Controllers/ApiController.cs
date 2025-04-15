using Microsoft.AspNetCore.Mvc;

namespace GatherBuddy.Web.Controllers;

public class ApiController : Controller
{
    private readonly ILogger<ApiController> _logger;
    public ApiController(ILogger<ApiController> logger)
    {

    }
}

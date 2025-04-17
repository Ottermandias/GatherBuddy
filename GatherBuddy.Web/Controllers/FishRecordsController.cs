using GatherBuddy.Models;
using GatherBuddy.Web.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GatherBuddy.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(ClientAccessControlFilter))]
[ServiceFilter(typeof(RateLimitFilter))]
public class FishRecordsController : Controller
{
    private readonly ILogger<FishRecordsController> _logger;
    private readonly GatherBuddyDbContext           _dbContext;

    public FishRecordsController(ILogger<FishRecordsController> logger, GatherBuddyDbContext dbContext)
    {
        _logger    = logger;
        _dbContext = dbContext;
    }

    [HttpGet]
    [Route("get")]
    public async Task<ActionResult<IEnumerable<SimpleFishRecord>>> GetFishRecords(int page = 0, int pageSize = 1000)
    {
        var records = _dbContext.FishRecords.OrderBy(r => r.Id).Skip(page * pageSize).Take(pageSize).ToListAsync();
        _logger.LogInformation($"Getting fish records page {page} of size {pageSize}");
        return Ok(await records);
    }

    [HttpGet]
    [Route("total")]
    public async Task<ActionResult<int>> GetFishRecordsTotal()
    {
        var total = await _dbContext.FishRecords.CountAsync();
        _logger.LogInformation($"Total fish records: {total}");
        return Ok(total);
    }

    [HttpPost]
    [Route("add")]
    public async Task<ActionResult<IEnumerable<SimpleFishRecord>>> AddFishRecords(List<SimpleFishRecord> records)
    {
        foreach (var record in records)
        {
            try
            {
                _logger.LogInformation($"Processing fish record {record.Id}");
                var existing = await _dbContext.FishRecords.FirstOrDefaultAsync(f => f.Id == record.Id);
                if (existing == null)
                {
                    _logger.LogInformation($"Adding fish record {record.Id}");
                    await _dbContext.FishRecords.AddAsync(record);
                }
                else
                {
                    _logger.LogInformation($"Updating fish record {record.Id}");
                    _dbContext.Entry(existing).CurrentValues.SetValues(record);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to add fish record {record.Id}");
            }
        }

        await _dbContext.SaveChangesAsync();
        return Ok(records);
    }

    [HttpDelete]
    [Route("delete")]
    public async Task<ActionResult> DeleteFishRecords(List<Guid> ids)
    {
        foreach (var id in ids)
        {
            _logger.LogInformation($"Deleting fish record {id}");
            var record = await _dbContext.FishRecords.FirstOrDefaultAsync(f => f.Id == id);
            if (record != null)
                _dbContext.FishRecords.Remove(record);
        }
        await _dbContext.SaveChangesAsync();
        return Ok();
    }
}

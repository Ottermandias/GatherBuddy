namespace GatherBuddy.Web.Database.Models;

public class RateLimitRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string IpAddress { get; set; } = string.Empty;
    public DateTime Expiry { get; set; }
}

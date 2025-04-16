namespace GatherBuddy.Web.Database.Models;

public class SecretKey
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Key { get; set; } = string.Empty;
    public DateTime Expiry { get; set; }
}

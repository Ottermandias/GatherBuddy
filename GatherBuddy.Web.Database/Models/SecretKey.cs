namespace GatherBuddy.Web.Database.Models;

public class SecretKey
{
    public SecretKey() { }
    public SecretKey(Guid key, DateTime expiry)
    {
        Key    = key;
        Expiry = expiry;
    }
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid Key { get; set; }
    public DateTime Expiry { get; set; }
}

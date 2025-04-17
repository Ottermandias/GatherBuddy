using GatherBuddy.Models;
using GatherBuddy.Web.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GatherBuddy.Web.Database;

public class GatherBuddyDbContext(DbContextOptions<GatherBuddyDbContext> options) : DbContext(options)
{
    public DbSet<SimpleFishRecord> FishRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

public class GatherBuddyDbContextFactory : IDesignTimeDbContextFactory<GatherBuddyDbContext>
{
    public GatherBuddyDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<GatherBuddyDbContext>();

        var dummyConnectionString = "Server=127.0.0.1;Database=DummyDb;User=DummyUser;Password=DummyPass;";
        builder.UseMySQL(dummyConnectionString);

        return new GatherBuddyDbContext(builder.Options);
    }
}

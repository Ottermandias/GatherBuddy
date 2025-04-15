using GatherBuddy.FishTimer;
using GatherBuddy.Web.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GatherBuddy.Web.Database;

public class GatherBuddyDbContext(DbContextOptions<GatherBuddyDbContext> options) : DbContext(options)
{
    public DbSet<FishRecord> FishRecords { get; set; }
    public DbSet<SecretKey>  SecretKeys  { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FishRecord>().Property<uint>("_bait").HasColumnName("BaitRaw");
        modelBuilder.Entity<FishRecord>().Property<uint>("_catch").HasColumnName("CatchRaw");
        modelBuilder.Entity<FishRecord>().Property<int>("_timeStamp").HasColumnName("Timestamp");
        modelBuilder.Entity<FishRecord>().Property<ushort>("_fishingSpot").HasColumnName("FishingSpot");
        modelBuilder.Entity<FishRecord>().Property<byte>("_tugAndHook").HasColumnName("TugAndHook");
        modelBuilder.Entity<FishRecord>().Property<float>("_x").HasColumnName("PositionX");
        modelBuilder.Entity<FishRecord>().Property<float>("_y").HasColumnName("PositionY");
        modelBuilder.Entity<FishRecord>().Property<float>("_z").HasColumnName("PositionZ");
        modelBuilder.Entity<FishRecord>().Property<float>("_rotation").HasColumnName("Rotation");
        modelBuilder.Entity<FishRecord>().Property<uint>("_worldId").HasColumnName("WorldId");
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

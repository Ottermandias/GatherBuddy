using GatherBuddy.Keys;
using GatherBuddy.Web.Controllers;
using GatherBuddy.Web.Database;
using GatherBuddy.Web.Database.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

namespace GatherBuddy.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        string? mysqlConnectionString = Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING");
        if (mysqlConnectionString == null)
            throw new Exception("MYSQL_CONNECTION_STRING environment variable not set.");

        builder.Services.AddDbContext<GatherBuddyDbContext>(options => { options.UseMySQL(mysqlConnectionString); });
        builder.Services.AddScoped<ApiKeyAuthFilter>();
        builder.Services.AddScoped<RateLimitFilter>();
        builder.Services.AddMemoryCache();
        builder.Logging.AddFilter("Microsoft.EntityFrameworkCore",       LogLevel.Warning);
        //builder.Logging.AddFilter("Microsoft.AspNetCore.Server.Kestrel", LogLevel.None);
        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor
              | ForwardedHeaders.XForwardedProto;
        });



        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<GatherBuddyDbContext>();
            db.Database.Migrate();
            Console.WriteLine("Migrated database.");

            var secretKey = SecretKeys.ApiKey;
            var keyRecord = db.SecretKeys.FirstOrDefault(k => k.Key == secretKey);
            if (keyRecord == null)
            {
                var newKeyRecord = new SecretKey();
                newKeyRecord.Key = secretKey;
                newKeyRecord.Expiry = DateTime.UtcNow.AddDays(60);
                db.SecretKeys.Add(newKeyRecord);
                db.SaveChanges();
                Console.WriteLine($"Added new secret key {secretKey} to database.");
            }
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseRouting();

        app.UseAuthorization();

        app.UseForwardedHeaders();

        app.MapStaticAssets();

        app.MapControllers()
            .WithStaticAssets();

        app.Run();
    }
}

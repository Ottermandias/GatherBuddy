using GatherBuddy.Web.Database;
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
        // if (mysqlConnectionString == null)
        //     throw new Exception("MYSQL_CONNECTION_STRING environment variable not set.");

        builder.Services.AddDbContext<GatherBuddyDbContext>(options => { options.UseMySQL(mysqlConnectionString); });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<GatherBuddyDbContext>();
            db.Database.Migrate();
        }

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        //app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();

        app.MapControllers()
            .WithStaticAssets();

        app.Run();
    }
}

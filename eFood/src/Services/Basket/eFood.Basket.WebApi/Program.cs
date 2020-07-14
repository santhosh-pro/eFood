using System.Reflection;
using eFood.Basket.WebApi.DAL;
using eFood.Common.InboxPattern.EntityFramework;
using eFood.Common.Migrator;
using eFood.Common.Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace eFood.Basket.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build()
                .MigrateToLatestVersion<BasketDbContext>((context, provider) =>
                {
                    //  seed data method here
                })
                .MigrateToLatestVersion<InboxDbContext>((context, provider) =>
                {
                    //  seed data method here
                }).Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .UseLogging(Assembly.GetExecutingAssembly())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

using Microsoft.Extensions.Hosting;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace eFood.Common.Migrator
{
    public static class HostBuilderExtensions
    {
        public static IHost MigrateToLatestVersion<TDbContext>(this IHost host,
            Action<TDbContext, IServiceProvider> seeder) where TDbContext : DbContext
        {
            Log.Debug("Start migrate data base");

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<TDbContext>();
                    context.Database.Migrate();
                    InvokeSeeder(seeder, context, services);

                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Can't migrate data base to latest version");

                    throw;
                }
            }

            Log.Debug("Complete migrate data base");

            return host;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context,
            IServiceProvider services)
            where TContext : DbContext
        {
            seeder(context, services);
        }
    }
}

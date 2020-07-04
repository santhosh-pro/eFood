using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace eFood.Common.Serilog
{
    public static class SerilogWebHostExtensions
    {
        public static IHostBuilder UseLogging(this IHostBuilder builder, Assembly executingAssembly)
        {
            builder.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration, (DependencyContext)null).Enrich
                    .WithProperty("Application", (object)executingAssembly.GetName().Name, false).Enrich
                    .WithProperty("Version", (object)executingAssembly.GetName().Version, false).Enrich
                    .WithMachineName().Enrich.FromLogContext();

                configuration
                    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                    .MinimumLevel.Override("System", LogEventLevel.Warning);
            });

            return builder;
        }
    }
}

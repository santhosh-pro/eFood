using System;
using eFood.Common.OutboxPattern.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eFood.Common.OutboxPattern
{
    public interface IOutboxConfiguration
    {
        IServiceCollection ServiceCollection { get; }
    }

    public class OutboxConfiguration : IOutboxConfiguration
    {
        public IServiceCollection ServiceCollection { get; }

        public OutboxConfiguration(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }
    }

    public static class OutboxMessageExtensions
    {
        public static IServiceCollection AddOutbox(this IServiceCollection services, IConfiguration configuration,
            Action<IOutboxConfiguration> inboxConfiguration)
        {
            var outboxConfig = configuration
                .GetSection("OutboxMessage")
                .Get<OutboxMessageConfiguration>();

            var configurator = new OutboxConfiguration(services);
            inboxConfiguration(configurator);
            services.AddSingleton(outboxConfig);

            return services;
        }

        public static IOutboxConfiguration AddEntityFramework<TDbContext>(this IOutboxConfiguration configuration) where TDbContext : DbContext
        {
            configuration.ServiceCollection.AddScoped<IOutboxMessageSave, EntityFrameworkAccessor<TDbContext>>();
            configuration.ServiceCollection.AddSingleton<IOutboxMessageAccessor, DapperOutboxAccessor>();
            configuration.ServiceCollection.AddHostedService<BackgroundMessagePublisher>();

            return configuration;
        }
    }
}

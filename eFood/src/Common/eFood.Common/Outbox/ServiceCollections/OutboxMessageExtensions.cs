using System;
using eFood.Common.MassTransit;
using eFood.Common.Outbox.MessageAccessors;
using eFood.Common.Outbox.MessageAccessors.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eFood.Common.Outbox.ServiceCollections
{
    public interface IOutboxAccessorConfiguration
    {
        IServiceCollection ServiceCollection { get; }
    }

    public class OutboxAccessorConfiguration : IOutboxAccessorConfiguration
    {
        public IServiceCollection ServiceCollection { get; }

        public OutboxAccessorConfiguration(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }
    }

    public static class OutboxMessageExtensions
    {
        public static IServiceCollection AddOutbox(this IServiceCollection services, IConfiguration configuration, Action<IOutboxAccessorConfiguration> accessConfiguration)
        {
            var outboxConfig = configuration
                .GetSection("OutboxMessage")
                .Get<OutboxMessageConfiguration>();

            var configurator = new OutboxAccessorConfiguration(services);
            accessConfiguration(configurator);

            services.AddSingleton(outboxConfig);

            return services;
        }

        public static IOutboxAccessorConfiguration AddInMemory(this IOutboxAccessorConfiguration configuration)
        {
            configuration.ServiceCollection.AddSingleton<IBusPublisher, MassTransitPublisher>();
            configuration.ServiceCollection.AddSingleton<IOutboxAccessor, InMemoryOutboxStorage>();
            configuration.ServiceCollection.AddSingleton<IOutboxMessageHandler, InMemoryOutboxStorage>();
            configuration.ServiceCollection.AddHostedService<OutboxMessageBackgroundProcessor.OutboxMessageBackgroundProcessor>();

            return configuration;
        }
    }

}

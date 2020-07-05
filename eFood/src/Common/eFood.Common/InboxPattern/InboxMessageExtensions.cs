using System;
using System.Collections.Generic;
using System.Text;
using eFood.Common.InboxPattern.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eFood.Common.InboxPattern
{
    public interface IInboxConfiguration
    {
        IServiceCollection ServiceCollection { get; }
    }

    public class InboxConfiguration : IInboxConfiguration
    {
        public IServiceCollection ServiceCollection { get; }

        public InboxConfiguration(IServiceCollection serviceCollection)
        {
            ServiceCollection = serviceCollection;
        }
    }

    public static class InboxMessageExtensions
    {
        public static IServiceCollection AddInbox(this IServiceCollection services, IConfiguration configuration,
            Action<IInboxConfiguration> inboxConfiguration)
        {

            var configurator = new InboxConfiguration(services);
            inboxConfiguration(configurator);


            return services;
        }

        public static IInboxConfiguration AddEntityFramework<TDbContext>(this IInboxConfiguration configuration) where TDbContext : DbContext
        {
            configuration.ServiceCollection.AddScoped<IInboxMessageProcessor, EntityFrameworkInboxProcessor<TDbContext>>();

            return configuration;
        }
    }
}

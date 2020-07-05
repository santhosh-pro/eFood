using System;
using System.Collections.Generic;
using System.Text;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eFood.Common.MassTransit
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration,
            Type type, string queueName)
        {
            var config = configuration.GetSection("MassTransitConfiguration")
                .Get<MassTransitConfiguration>();


            services.AddSingleton(config);
            services.AddMassTransit(x =>
            {
                if (type != null)
                    x.AddConsumer(type);

                
                var baseUri = new Uri(config.RabbitMQAddress);
                var hostUri = new Uri(baseUri, config.HostName);
                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {


                    cfg.Host(hostUri, hostCfg =>
                    {
                        hostCfg.Username(config.UserName);
                        hostCfg.Password(config.Password);
                    });

                    if (type != null)
                    {
                        cfg.ReceiveEndpoint(queueName, e =>
                        {
                            e.ConfigureConsumer(context, type);
                        });
                    }


                }));
            });

            services.AddMassTransitHostedService();
            return services;
        }
    }
}

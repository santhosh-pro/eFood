using System;
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
            var config = configuration.GetSection("MassTransitHostConfiguration")
                .Get<MassTransitHostConfiguration>();


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
    public class MassTransitHostConfiguration
    {
        public string RabbitMQAddress { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
    }
}

using Archse.Consumer;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Archse.Extensions
{

    public static class MasstransitExtension
    {
        public static void AddMassTransitConsumer(this IServiceCollection services,
            IConfiguration configuration)
        {
            string connectionString = configuration.GetSection("ConnectionStrings:ServiceBus").Value.ToString();
            services.AddMassTransit(x =>
            {
                x.AddConsumer<QueueGameInsertedConsumer>();

                x.UsingAzureServiceBus((context, cfg) =>
                {
                    cfg.Host(connectionString);

                    cfg.ReceiveEndpoint("game-inserted", e =>
                    {
                        e.ConfigureConsumer<QueueGameInsertedConsumer>(context);
                    });
                });
            });

            services.AddMassTransitHostedService();
        }
    }

}
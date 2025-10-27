using Azure.Messaging.ServiceBus;
using InstaDelivery.DeliveryService.Messaging.Producers.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InstaDelivery.DeliveryService.Messaging;

public static class ConfigurationExtensions
{
    public static IServiceCollection ConfigureMessagingServices(this IServiceCollection services)
    {

        services.AddKeyedSingleton<ServiceBusClient>("OrderServiceBus", (sp, key) =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("OrderServiceBus");
            return new ServiceBusClient(connectionString);
        });

        services.AddSingleton<IOrderEventProducer, OrderEventProducer>();
        return services;
    }
}

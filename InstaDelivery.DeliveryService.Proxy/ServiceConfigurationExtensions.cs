using InstaDelivery.DeliveryService.Proxy.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InstaDelivery.DeliveryService.Proxy;

public static class ServiceConfigurationExtensions
{
    public static IServiceCollection ConfigureProxyServices(this IServiceCollection services)
    {

        services.AddHttpClient<IOrderServiceClient, OrderServiceClient>((sp, client) =>
        {
            var baseUri = sp.GetRequiredService<IConfiguration>()["ApiServices:OrderService:BaseUrl"]!;

            client.BaseAddress = new Uri(baseUri);
        });
        return services;
    }
}

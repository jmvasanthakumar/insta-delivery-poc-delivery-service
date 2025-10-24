using InstaDelivery.DeliveryService.Application.Contracts;
using InstaDelivery.DeliveryService.Application.MapperProfiles;
using InstaDelivery.DeliveryService.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InstaDelivery.DeliveryService.Application;

public static class ServicesConfigurationExtension
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(
            cfg => { },
            typeof(MapperProfile).Assembly
        );

        services.AddScoped<IOrderService, OrderService>(); 
        services.AddScoped<IDeliveryService, Services.DeliveryService>(); 
        return services;
    }
}
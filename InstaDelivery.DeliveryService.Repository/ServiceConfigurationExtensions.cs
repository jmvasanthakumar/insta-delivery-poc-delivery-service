using InstaDelivery.DeliveryService.Repository.Context;
using InstaDelivery.DeliveryService.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InstaDelivery.DeliveryService.Repository;

public static class ServiceConfigurationExtensions
{
    public static IServiceCollection ConfigureRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DeliveryDb");

        services.AddDbContext<DeliveryDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IDeliveryAgentRepository, DeliveryAgentRepository>();
        services.AddScoped<IDeliveryRepository, DeliveryRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}

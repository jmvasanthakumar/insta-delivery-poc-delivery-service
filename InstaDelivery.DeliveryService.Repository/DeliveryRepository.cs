using InstaDelivery.DeliveryService.Domain.Entities;
using InstaDelivery.DeliveryService.Repository.Context;
using InstaDelivery.DeliveryService.Repository.Contracts;

namespace InstaDelivery.DeliveryService.Repository;

internal class DeliveryRepository(DeliveryDbContext dbContext) : 
    GenericRepository<Delivery>(dbContext), IDeliveryRepository
{
}

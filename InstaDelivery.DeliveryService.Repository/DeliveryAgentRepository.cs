using InstaDelivery.DeliveryService.Domain.Entities;
using InstaDelivery.DeliveryService.Repository.Context;
using InstaDelivery.DeliveryService.Repository.Contracts;

namespace InstaDelivery.DeliveryService.Repository;

internal class DeliveryAgentRepository(DeliveryDbContext dbContext) : 
    GenericRepository<DeliveryAgent>(dbContext), IDeliveryAgentRepository
{
}

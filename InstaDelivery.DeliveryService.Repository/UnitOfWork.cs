using InstaDelivery.DeliveryService.Repository.Context;
using InstaDelivery.DeliveryService.Repository.Contracts;

namespace InstaDelivery.DeliveryService.Repository;

internal class UnitOfWork(
    DeliveryDbContext db,
    IDeliveryRepository delivery,
    IDeliveryAgentRepository deliveryAgents) : IUnitOfWork
{
    public IDeliveryAgentRepository DeliveryAgent { get; } = deliveryAgents;
    public IDeliveryRepository Delivery { get; } = delivery;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await db.SaveChangesAsync(ct);

    public void Dispose() => db.Dispose();
}

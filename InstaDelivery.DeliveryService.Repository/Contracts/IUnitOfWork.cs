namespace InstaDelivery.DeliveryService.Repository.Contracts;

public interface IUnitOfWork
{
    public IDeliveryAgentRepository DeliveryAgent { get; }
    public IDeliveryRepository Delivery{ get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
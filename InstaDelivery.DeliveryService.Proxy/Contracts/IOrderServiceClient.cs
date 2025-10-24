using InstaDelivery.DeliveryService.Proxy.Response;

namespace InstaDelivery.DeliveryService.Proxy.Contracts;

public interface IOrderServiceClient
{
    Task<List<AvailableOrder>> GetAvailableOrdersAsync(CancellationToken ct = default);
    Task<OrderDetail> GetOrderDetailsAsync(Guid orderId, CancellationToken ct = default);
}

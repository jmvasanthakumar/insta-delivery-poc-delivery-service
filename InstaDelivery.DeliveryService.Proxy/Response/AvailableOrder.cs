using InstaDelivery.DeliveryService.Domain.Enum;

namespace InstaDelivery.DeliveryService.Proxy.Response;

public class AvailableOrder
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
}

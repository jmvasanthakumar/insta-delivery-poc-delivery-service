using InstaDelivery.DeliveryService.Messaging.Contracts;

namespace InstaDelivery.DeliveryService.Messaging.Producers.Contracts;

public interface IOrderEventProducer
{
    Task PushOrderEventAsync(OrderStatusChange orderEvent);
}

namespace InstaDelivery.DeliveryService.Messaging.Contracts;

public class OrderStatusChange
{
    public Guid OrderId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTimeOffset ChangedAt { get; set; }
}

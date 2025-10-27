namespace InstaDelivery.DeliveryService.Domain.Enum;

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Assigned = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5,
}
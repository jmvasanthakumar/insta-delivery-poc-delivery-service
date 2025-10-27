namespace InstaDelivery.DeliveryService.Domain.Exceptions;

public class DeliveryRecordNotFoundException : Exception
{
    public DeliveryRecordNotFoundException(Guid orderId)
        : base($"Delivery record not found for order ID: {orderId}")
    {
    }
}

namespace InstaDelivery.DeliveryService.Domain.Exceptions;

public class DeliveryPartnerNotFoundException : Exception
{
    public DeliveryPartnerNotFoundException(Guid partnerId)
        : base($"Delivery partner with ID '{partnerId}' was not found.")
    {
    }
}

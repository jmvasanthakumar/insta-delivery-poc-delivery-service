namespace InstaDelivery.DeliveryService.Application.Dto;

public class DeliveryStatusDto
{
    public Guid OrderId { get; set; }
    public string Status { get; set; } = string.Empty;
}

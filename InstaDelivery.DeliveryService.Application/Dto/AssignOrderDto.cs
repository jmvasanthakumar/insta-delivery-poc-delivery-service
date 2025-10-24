namespace InstaDelivery.DeliveryService.Application.Dto;

public class AssignOrderDto
{
    public Guid OrderId { get; set; }
    public Guid PartnerId { get; set; }
}
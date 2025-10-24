namespace InstaDelivery.DeliveryService.Application.Dto
{
    public class AvailableOrderDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}

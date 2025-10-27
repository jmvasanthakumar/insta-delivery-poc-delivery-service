namespace InstaDelivery.DeliveryService.Proxy.Response
{
    public class OrderDetail
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public OrderAddress DeliveryAddress { get; set; } = new OrderAddress();

    }
}

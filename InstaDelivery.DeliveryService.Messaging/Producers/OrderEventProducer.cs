using Azure.Messaging.ServiceBus;
using InstaDelivery.DeliveryService.Messaging.Contracts;
using InstaDelivery.DeliveryService.Messaging.Producers.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

internal class OrderEventProducer : IOrderEventProducer
{
    private readonly ServiceBusClient _client;
    private readonly ServiceBusSender _sender;

    public OrderEventProducer([FromKeyedServices("OrderServiceBus")] ServiceBusClient client)
    {
        _client = client;
        _sender = _client.CreateSender("order-events");
    }

    public async Task PushOrderEventAsync(OrderStatusChange orderEvent)
    {
        string messageBody = JsonSerializer.Serialize(orderEvent);

        var message = new ServiceBusMessage(messageBody)
        {
            ContentType = "application/json"
        };

        await _sender.SendMessageAsync(message);
    }
}

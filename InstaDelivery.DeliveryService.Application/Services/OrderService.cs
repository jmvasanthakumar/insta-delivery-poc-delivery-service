using AutoMapper;
using InstaDelivery.DeliveryService.Application.Contracts;
using InstaDelivery.DeliveryService.Application.Dto;
using InstaDelivery.DeliveryService.Proxy.Contracts;
using InstaDelivery.DeliveryService.Proxy.Response;

namespace InstaDelivery.DeliveryService.Application.Services;

internal class OrderService(IOrderServiceClient orderServiceClient, IMapper mapper) : IOrderService
{
    public async Task<IList<AvailableOrderDto>> GetAvailableOrderAsync(CancellationToken ct = default)
    {
        var availableOrders = await orderServiceClient.GetAvailableOrdersAsync(ct);
        return mapper.Map<IList<AvailableOrderDto>>(availableOrders);
    }
}

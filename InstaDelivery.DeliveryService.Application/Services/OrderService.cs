using AutoMapper;
using InstaDelivery.DeliveryService.Application.Dto;
using InstaDelivery.DeliveryService.Application.Services.Contracts;
using InstaDelivery.DeliveryService.Proxy.Contracts;

namespace InstaDelivery.DeliveryService.Application.Services;

internal class OrderService(IOrderServiceClient orderServiceClient, IMapper mapper) : IOrderService
{
    public async Task<IList<AvailableOrderDto>> GetAvailableOrderAsync(CancellationToken ct = default)
    {
        var availableOrders = await orderServiceClient.GetAvailableOrdersAsync(ct);
        return mapper.Map<IList<AvailableOrderDto>>(availableOrders);
    }
}

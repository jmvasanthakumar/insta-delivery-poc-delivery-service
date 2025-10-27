using InstaDelivery.DeliveryService.Application.Dto;
using InstaDelivery.DeliveryService.Proxy.Response;

namespace InstaDelivery.DeliveryService.Application.Services.Contracts;

public interface IOrderService
{
    Task<IList<AvailableOrderDto>> GetAvailableOrderAsync(CancellationToken ct = default);
}

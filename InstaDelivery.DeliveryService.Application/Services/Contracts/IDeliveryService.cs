using InstaDelivery.DeliveryService.Application.Dto;

namespace InstaDelivery.DeliveryService.Application.Services.Contracts;

public interface IDeliveryService
{
    Task<bool> AssignOrderAsync(AssignOrderDto dto, CancellationToken ct);
    Task UpdateDeliveryStatusAsync(DeliveryStatusDto dto, CancellationToken ct);
}

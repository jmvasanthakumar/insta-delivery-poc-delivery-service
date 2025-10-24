using InstaDelivery.DeliveryService.Application.Dto;

namespace InstaDelivery.DeliveryService.Application.Contracts;

public interface IDeliveryService
{
    Task<bool> AssignOrderAsync(AssignOrderDto dto, CancellationToken ct);
}

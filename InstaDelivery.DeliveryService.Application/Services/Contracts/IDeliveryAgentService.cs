using InstaDelivery.DeliveryService.Application.Dto;

namespace InstaDelivery.DeliveryService.Application.Services.Contracts;

public interface IDeliveryAgentService
{
    Task<DeliveryAgentDto> RegisterDeliveryAgentAsync(CreateDeliveryAgentDto deliveryAgentDto, CancellationToken ct);
    Task UpdateDeliveryAgentStatusAsync(DeliveryAgentStatusDto deliveryAgentstatusDto, CancellationToken ct);
    Task<DeliveryAgentDto> UpdateDeliveryAgentAsync(DeliveryAgentDto deliveryAgentDto, CancellationToken ct);
}

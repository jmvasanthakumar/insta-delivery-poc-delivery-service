using InstaDelivery.DeliveryService.Application.Dto;

namespace InstaDelivery.DeliveryService.Application.Services.Contracts;

public interface IDeliveryAgentService
{
    Task<IList<DeliveryAgentDto>> GetAllDeliveryAgentsAsync(CancellationToken ct = default);
    Task<DeliveryAgentDto> RegisterDeliveryAgentAsync(CreateDeliveryAgentDto deliveryAgentDto, CancellationToken ct = default);
    Task UpdateDeliveryAgentStatusAsync(DeliveryAgentStatusDto deliveryAgentstatusDto, CancellationToken ct = default);
    Task<DeliveryAgentDto> UpdateDeliveryAgentAsync(DeliveryAgentDto deliveryAgentDto, CancellationToken ct = default);
}

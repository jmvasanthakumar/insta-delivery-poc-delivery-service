using AutoMapper;
using InstaDelivery.DeliveryService.Application.Contracts;
using InstaDelivery.DeliveryService.Application.Dto;
using InstaDelivery.DeliveryService.Domain;
using InstaDelivery.DeliveryService.Domain.Entities;
using InstaDelivery.DeliveryService.Domain.Exceptions;
using InstaDelivery.DeliveryService.Proxy.Contracts;
using InstaDelivery.DeliveryService.Repository.Contracts;

namespace InstaDelivery.DeliveryService.Application.Services;

internal class DeliveryService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IOrderServiceClient orderServiceClient) : IDeliveryService
{
    public async Task<DeliveryAgentDto> RegisterDeliveryAgentAsync(CreateDeliveryAgentDto deliveryAgentDto, CancellationToken ct)
    {
        var entity = mapper.Map<DeliveryAgent>(deliveryAgentDto);

        var isExisting = await unitOfWork.DeliveryAgent.AnyAsync(x => x.Email == deliveryAgentDto.Email && x.PhoneNumber == deliveryAgentDto.PhoneNumber, ct);

        if (isExisting)
        {
            throw new DeliveryAgentDetailsAlreadyExistException(deliveryAgentDto.PhoneNumber, deliveryAgentDto.Email);
        }

        await unitOfWork.DeliveryAgent.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<DeliveryAgentDto>(entity);
    }

    public async Task<bool> AssignOrderAsync(AssignOrderDto dto, CancellationToken ct)
    {
        var deliveryAgent = await unitOfWork.DeliveryAgent.GetByIdAsync(dto.PartnerId, ct)
            ?? throw new DeliveryPartnerNotFoundException(dto.PartnerId);

        if (deliveryAgent.Capacity <= 0 || !deliveryAgent.IsOnline)
        {
            return false;
        }

        var isAssigned = await unitOfWork.Delivery.AnyAsync(x => x.OrderId == dto.OrderId && x.Status == DeliveryStatus.Assigned, ct);

        if (isAssigned)
        {
            return false;
        }

        var orderDetails = await orderServiceClient.GetOrderDetailsAsync(dto.OrderId, ct);

        var delivery = new Delivery
        {
            OrderId = dto.OrderId,
            DeliveryAgentId = dto.PartnerId,
            DeliveryAddress = orderDetails.DeliveryAddress,
            AssignedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            CreatedAt = DateTimeOffset.UtcNow,
            Status = DeliveryStatus.Assigned
        };

        await unitOfWork.Delivery.AddAsync(delivery, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return true;
    }
}

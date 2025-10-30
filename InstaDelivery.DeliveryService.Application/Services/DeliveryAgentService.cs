using AutoMapper;
using InstaDelivery.DeliveryService.Application.Dto;
using InstaDelivery.DeliveryService.Application.Services.Contracts;
using InstaDelivery.DeliveryService.Domain.Entities;
using InstaDelivery.DeliveryService.Domain.Exceptions;
using InstaDelivery.DeliveryService.Repository.Contracts;

namespace InstaDelivery.DeliveryService.Application.Services;

internal class DeliveryAgentService(IUnitOfWork unitOfWork,
    IMapper mapper) : IDeliveryAgentService
{

    public async Task<IList<DeliveryAgentDto>> GetAllDeliveryAgentsAsync(CancellationToken ct)
    {
        var entities = await unitOfWork.DeliveryAgent.GetAllAsync(ct);
        return mapper.Map<IList<DeliveryAgentDto>>(entities);
    }

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

    public async Task<DeliveryAgentDto> UpdateDeliveryAgentAsync(DeliveryAgentDto deliveryAgentDto, CancellationToken ct)
    {
        var entity = (await unitOfWork.DeliveryAgent.FindAsync(x => x.Id == deliveryAgentDto.Id, ct)).SingleOrDefault()
            ?? throw new DeliveryPartnerNotFoundException(deliveryAgentDto.Id);

        unitOfWork.DeliveryAgent.Update(entity);
        await unitOfWork.SaveChangesAsync(ct);

        return mapper.Map<DeliveryAgentDto>(entity);
    }

    public async Task UpdateDeliveryAgentStatusAsync(DeliveryAgentStatusDto deliveryAgentstatusDto, CancellationToken ct)
    {
        var entity = (await unitOfWork.DeliveryAgent.FindAsync(x => x.Id == deliveryAgentstatusDto.Id, ct)).SingleOrDefault()
            ?? throw new DeliveryPartnerNotFoundException(deliveryAgentstatusDto.Id);

        entity.Status = deliveryAgentstatusDto.Status;

        unitOfWork.DeliveryAgent.Update(entity);
        await unitOfWork.SaveChangesAsync(ct);
    }
}

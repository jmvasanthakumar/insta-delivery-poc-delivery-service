using InstaDelivery.DeliveryService.Application.Dto;
using InstaDelivery.DeliveryService.Application.Services.Contracts;
using InstaDelivery.DeliveryService.Domain;
using InstaDelivery.DeliveryService.Domain.Entities;
using InstaDelivery.DeliveryService.Domain.Exceptions;
using InstaDelivery.DeliveryService.Messaging.Contracts;
using InstaDelivery.DeliveryService.Messaging.Producers.Contracts;
using InstaDelivery.DeliveryService.Proxy.Contracts;
using InstaDelivery.DeliveryService.Repository.Contracts;
using Newtonsoft.Json;

namespace InstaDelivery.DeliveryService.Application.Services;

internal class DeliveryService(
    IUnitOfWork unitOfWork,
    IOrderServiceClient orderServiceClient,
    IOrderEventProducer orderEventProducer) : IDeliveryService
{


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
            DeliveryAddress = JsonConvert.SerializeObject(orderDetails.DeliveryAddress),
            AssignedAt = DateTimeOffset.Now,
            UpdatedAt = DateTimeOffset.Now,
            CreatedAt = DateTimeOffset.Now,
            Status = DeliveryStatus.Assigned
        };

        await unitOfWork.Delivery.AddAsync(delivery, ct);
        await unitOfWork.SaveChangesAsync(ct);

        await orderEventProducer.PushOrderEventAsync(new OrderStatusChange
        {
            OrderId = dto.OrderId,
            Status = OrderStatus.Assigned.ToString(),
            ChangedAt = DateTimeOffset.Now
        });
        return true;
    }

    public async Task UpdateDeliveryStatusAsync(DeliveryStatusDto dto, CancellationToken ct)
    {
        var delivery = (await unitOfWork.Delivery.FindAsync(x => x.OrderId == dto.OrderId, ct)).SingleOrDefault()
            ?? throw new DeliveryRecordNotFoundException(dto.OrderId);

        delivery.Status = dto.Status;

        if (dto.Status == DeliveryStatus.Delivered)
        {
            delivery.DeliveredAt = DateTimeOffset.Now;
        }

        unitOfWork.Delivery.Update(delivery);
        await unitOfWork.SaveChangesAsync(ct);

        if (dto.Status == DeliveryStatus.Delivered || dto.Status == DeliveryStatus.Cancelled)
        {
            var deliveryAgent = await unitOfWork.DeliveryAgent.GetByIdAsync(delivery.DeliveryAgentId!.Value, ct)
                ?? throw new DeliveryPartnerNotFoundException(delivery.DeliveryAgentId.Value);

            deliveryAgent.Capacity += 1;
            unitOfWork.DeliveryAgent.Update(deliveryAgent);
            await unitOfWork.SaveChangesAsync(ct);

            await orderEventProducer.PushOrderEventAsync(new OrderStatusChange
            {
                OrderId = dto.OrderId,
                Status = dto.Status == DeliveryStatus.Delivered ? OrderStatus.Delivered.ToString() : OrderStatus.Pending.ToString(),
                ChangedAt = DateTimeOffset.Now
            });
        }
    }
}
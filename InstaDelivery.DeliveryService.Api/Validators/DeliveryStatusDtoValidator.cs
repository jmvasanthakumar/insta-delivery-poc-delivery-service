using FluentValidation;
using InstaDelivery.DeliveryService.Application.Dto;

namespace InstaDelivery.DeliveryService.Api.Validators;

public class DeliveryStatusDtoValidator : AbstractValidator<DeliveryStatusDto>
{
    public DeliveryStatusDtoValidator()
    {
        var allowedStatuses = new[] { "Assigned", "InTransit", "Delivered", "Cancelled", "Pending" };

        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderId is required.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(status => allowedStatuses.Contains(status))
            .WithMessage($"Status must be one of the following: {string.Join(", ", allowedStatuses)}");
    }
}

using FluentValidation;
using InstaDelivery.DeliveryService.Application.Dto;

namespace InstaDelivery.DeliveryService.Api.Validators
{
    public class DeliveryAgentStatusDtoValidator : AbstractValidator<DeliveryAgentStatusDto>
    {
        public DeliveryAgentStatusDtoValidator()
        {
            var allowedStatuses = new[] { "Online", "Offline", "Suspended" };

            RuleFor(x => x.Status)
                .Must(status => allowedStatuses.Contains(status))
                .WithMessage($"Status must be one of the following values: {string.Join(", ", allowedStatuses)}");
        }
    }
}

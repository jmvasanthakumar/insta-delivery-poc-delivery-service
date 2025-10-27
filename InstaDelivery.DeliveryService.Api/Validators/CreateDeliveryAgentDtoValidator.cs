using FluentValidation;
using InstaDelivery.DeliveryService.Application.Dto;

namespace InstaDelivery.DeliveryService.Api.Validators;

public class CreateDeliveryAgentDtoValidator : AbstractValidator<CreateDeliveryAgentDto>
{
    public CreateDeliveryAgentDtoValidator()
    {
        var allowedStatuses = new[] { "Online", "Offline", "Suspended" };

        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Email) || !string.IsNullOrWhiteSpace(x.PhoneNumber))
            .WithMessage("Either email or phone number is required.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.Email));

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{9,14}$")
            .WithMessage("Phone number must be valid and include country code, e.g., +14155552671")
            .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

        RuleFor(x => x.Status)
            .Must(status => allowedStatuses.Contains(status))
            .WithMessage($"Status must be one of the following values: {string.Join(", ", allowedStatuses)}");
    }
}

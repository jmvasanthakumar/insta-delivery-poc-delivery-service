using FluentValidation;
using InstaDelivery.DeliveryService.Application.Dto;

namespace InstaDelivery.DeliveryService.Api.Validators
{
    public class CreateDeliveryAgentDtoValidator : AbstractValidator<CreateDeliveryAgentDto>
    {
        public CreateDeliveryAgentDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            //RuleFor(x => x.PhoneNumber).NotNull().();
        }
    }
}

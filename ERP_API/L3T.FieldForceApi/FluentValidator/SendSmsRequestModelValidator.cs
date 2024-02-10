using FluentValidation;
using L3T.Infrastructure.Helpers.Models.FieldForce.RequestModels;

namespace L3T.FieldForceApi.FluentValidator
{
    public class SendSmsRequestModelValidator : AbstractValidator<SendSmsRequestModel>
    {
        public SendSmsRequestModelValidator()
        {
            //RuleFor(p => p.FirstName)
            //    .Cascade(CascadeMode.StopOnFirstFailure)
            //    .NotEmpty().WithMessage("{PropertyName} should be not empty. NEVER!")
            //    .Length(2, 25)
            //    .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");

            RuleFor(p => p.TicketRefNo)
                .NotEmpty().WithMessage("{PropertyName} should not be empty!");
        }
    }
}

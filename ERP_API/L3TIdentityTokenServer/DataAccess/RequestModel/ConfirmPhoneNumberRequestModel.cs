using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace L3TIdentityTokenServer.DataAccess.RequestModel
{
    public class ConfirmPhoneNumberRequestModel
    {
        public string Token { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class ConfirmPhoneNumberRequestModelValidator : AbstractValidator<ConfirmPhoneNumberRequestModel>
    {
        public ConfirmPhoneNumberRequestModelValidator()
        {
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Token).NotEmpty();
        }
    }
}

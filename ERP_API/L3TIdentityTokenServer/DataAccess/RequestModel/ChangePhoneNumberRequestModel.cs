using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace L3TIdentityTokenServer.DataAccess.RequestModel
{
    public class ChangePhoneNumberRequestModel
    {
        public string PhoneNumber { get; set; }
    }
    public class ChangePhoneNumberRequestModelValidator : AbstractValidator<ChangePhoneNumberRequestModel>
    {
        public ChangePhoneNumberRequestModelValidator()
        {
            RuleFor(x => x.PhoneNumber).NotEmpty();
        }
    }
}

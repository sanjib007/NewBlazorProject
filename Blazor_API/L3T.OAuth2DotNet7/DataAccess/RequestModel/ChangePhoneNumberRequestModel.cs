using FluentValidation;

namespace L3T.OAuth2DotNet7.DataAccess.RequestModel
{
    public class ChangePhoneNumberRequestModel
    {
        public string PhoneNumber { get; set; }
    }
    public class ChangePhoneNumberRequestModelValidator : AbstractValidator<ChangePhoneNumberRequestModel>
    {
        public ChangePhoneNumberRequestModelValidator()
        {
            RuleFor(x => x.PhoneNumber).NotEmpty().NotNull();
        }
    }
}

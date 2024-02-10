using FluentValidation;

namespace L3T.OAuth2DotNet7.DataAccess.RequestModel
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
            RuleFor(x => x.PhoneNumber).NotEmpty().NotNull();
            RuleFor(x => x.Token).NotEmpty().NotNull();
        }
    }
}

using FluentValidation;

namespace L3T.OAuth2DotNet7.DataAccess.RequestModel
{
    public class RegisterRequestModel
    {
        public string UserName { get; set; }
        public string? Password { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Role { get; set; }
        public bool? IsLoginWithAD { get; set; }

    }

    public class RegisterViewModelValidator : AbstractValidator<RegisterRequestModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().NotNull();
            //RuleFor(x => x.Password).NotEmpty().NotNull();
            RuleFor(x => x.Email).EmailAddress().NotNull();
            //RuleFor(x => x.PhoneNumber).NotEmpty().NotNull();
            //RuleFor(x => x.Role).NotEmpty().NotNull();
        }
    }
}
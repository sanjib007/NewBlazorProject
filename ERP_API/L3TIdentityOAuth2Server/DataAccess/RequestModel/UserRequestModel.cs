using FluentValidation;

namespace L3TIdentityOAuth2Server.DataAccess.RequestModel
{
    public class UserRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool EmailConfirm { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirm { get; set; }
        public string FullName { get; set; }
        public string L3ID { get; set; }
        public List<string> RoleName { get; set; }
        public bool IsActive { get; set; }

    }
    
    public class UserRequestModelValidator : AbstractValidator<UserRequestModel>
    {
        public UserRequestModelValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().NotNull();
            RuleFor(x => x.Email).NotEmpty().NotNull();
            RuleFor(x => x.EmailConfirm).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.PhoneNumberConfirm).NotEmpty();
            RuleFor(x => x.FullName).NotEmpty();
            RuleFor(x => x.L3ID).NotEmpty();
            RuleFor(x => x.IsActive).NotEmpty();
            RuleFor(x => x.RoleName).NotEmpty();
        }
    }
}

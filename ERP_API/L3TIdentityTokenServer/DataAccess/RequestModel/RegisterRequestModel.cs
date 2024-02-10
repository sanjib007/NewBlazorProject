using FluentValidation;

namespace L3TIdentityTokenServer.DataAccess.RequestModel
{
    public class RegisterRequestModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        
    }
    
    public class RegisterViewModelValidator : AbstractValidator<RegisterRequestModel> 
    {
        public RegisterViewModelValidator() 
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Role).NotEmpty();
        }
    }
}
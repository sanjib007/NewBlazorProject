using FluentValidation;

namespace L3T.OAuth2DotNet7.DataAccess.RequestModel;

public class UpdateUserRequestModel
{
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public string FullName { get; set; }
    public List<string> RoleName { get; set; }
    public bool IsActive { get; set; }
}

public class UpdateUserRequestModelValidator : AbstractValidator<UpdateUserRequestModel>
{
    public UpdateUserRequestModelValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.EmailConfirmed).NotEmpty();
        RuleFor(x => x.PhoneNumber).NotEmpty();
        RuleFor(x => x.PhoneNumberConfirmed).NotEmpty();
        RuleFor(x => x.FullName).NotEmpty();
        RuleFor(x => x.IsActive).NotEmpty();
        RuleFor(x => x.RoleName).NotEmpty();
    }
}
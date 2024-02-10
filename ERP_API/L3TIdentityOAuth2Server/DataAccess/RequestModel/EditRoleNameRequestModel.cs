using FluentValidation;

namespace L3TIdentityOAuth2Server.DataAccess.RequestModel;

public class EditRoleNameRequestModel
{
    public string  Id { get; set; }
    public string RoleName { get; set; }
}

public class EditRoleNameRequestModelValidator : AbstractValidator<EditRoleNameRequestModel>
{
    public EditRoleNameRequestModelValidator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.RoleName).NotEmpty().NotNull();
    }
}
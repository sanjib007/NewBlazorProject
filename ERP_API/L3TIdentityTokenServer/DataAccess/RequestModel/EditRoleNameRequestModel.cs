using FluentValidation;

namespace L3TIdentityTokenServer.DataAccess.RequestModel;

public class EditRoleNameRequestModel
{
    public string  Id { get; set; }
    public string RoleName { get; set; }
}

public class EditRoleNameRequestModelValidator : AbstractValidator<EditRoleNameRequestModel>
{
    public EditRoleNameRequestModelValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RoleName).NotEmpty();
    }
}
using FluentValidation;

namespace L3T.OAuth2DotNet7.DataAccess.RequestModel;

public class EditRoleNameRequestModel
{
    public string Id { get; set; }
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
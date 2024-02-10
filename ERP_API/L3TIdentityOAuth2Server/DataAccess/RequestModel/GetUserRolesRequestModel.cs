using FluentValidation;

namespace L3TIdentityOAuth2Server.DataAccess.RequestModel;

public class GetUserRolesRequestModel
{
    public string UserName { get; set; }
    public string RoleName { get; set; }
    public bool PagingMode { get; set; } = false;
}

public class GetUserRolesRequestModelValidator : AbstractValidator<GetUserRolesRequestModel>
{
    public GetUserRolesRequestModelValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().NotNull();
        RuleFor(x => x.RoleName).NotEmpty().NotNull();
    }
}
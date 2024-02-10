using FluentValidation;

namespace L3TIdentityOAuth2Server.DataAccess.RequestModel;

public class GetUserClaimsRequestModel
{
    public string UserName { get; set; }
    public string? ClaimName { get; set; }
    public string? ClaimType { get; set; }
    public bool PagingMode { get; set; } = false;
}

public class GetUserClaimsRequestModelValidator : AbstractValidator<GetUserClaimsRequestModel>
{
    public GetUserClaimsRequestModelValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().NotNull();
    }
}
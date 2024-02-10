using FluentValidation;

namespace L3T.OAuth2DotNet7.DataAccess.RequestModel;

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
using FluentValidation;

namespace L3T.OAuth2DotNet7.DataAccess.RequestModel;

public class ResetPasswordModel
{
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}

public class ResetPasswordModelValidator : AbstractValidator<ResetPasswordModel>
{
    public ResetPasswordModelValidator()
    {
        RuleFor(x => x.NewPassword).NotEmpty();
        RuleFor(x => x.ConfirmPassword).NotEmpty();
        RuleFor(x => x.NewPassword)
            .Equal(d => d.ConfirmPassword).WithMessage("New Password must be equal confirm password");
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Token).NotEmpty();
    }
}
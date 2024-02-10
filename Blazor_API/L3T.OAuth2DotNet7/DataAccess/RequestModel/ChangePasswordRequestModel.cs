using FluentValidation;

namespace L3T.OAuth2DotNet7.DataAccess.RequestModel;

public class ChangePasswordRequestModel
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmPassword { get; set; }
}

public class ChangePasswordRequestModelValidator : AbstractValidator<ChangePasswordRequestModel>
{
    public ChangePasswordRequestModelValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty().NotNull();
        RuleFor(x => x.NewPassword).NotEmpty().NotNull();
        RuleFor(x => x.ConfirmPassword).NotEmpty().NotNull();
        RuleFor(x => x.NewPassword)
            .Equal(d => d.ConfirmPassword).WithMessage("New Password must be equal confirm password");
    }
}
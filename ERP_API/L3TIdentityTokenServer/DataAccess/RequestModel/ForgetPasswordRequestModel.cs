using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace L3TIdentityTokenServer.DataAccess.RequestModel;

public class ForgetPasswordRequestModel
{
    public string Email { get; set; }
}

public class ForgetPasswordRequestModelValidator : AbstractValidator<ForgetPasswordRequestModel>
{
    public ForgetPasswordRequestModelValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
    }
}
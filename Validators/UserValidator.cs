using FluentValidation;
using System;
using ViewModels.Home;

namespace Validators
{
    public class UserValidator : AbstractValidator<UserViewModel>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(50);
        }
    }
}

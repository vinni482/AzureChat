using FluentValidation;
using System;
using ViewModels.Home;

namespace Validators
{
    public class MessageValidator : AbstractValidator<MessageViewModel>
    {
        public MessageValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Message).NotEmpty().MaximumLength(200);
        }
    }
}

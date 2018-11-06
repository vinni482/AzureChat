using FluentValidation;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Validators;
using ViewModels.Home;

namespace AzureChat
{
    public class ValidatorFactory : ValidatorFactoryBase
    {
        private readonly IKernel kernel;

        public ValidatorFactory()
        {
            kernel = new StandardKernel();
            kernel.Bind<IValidator<UserViewModel>>().To<UserValidator>();
            kernel.Bind<IValidator<MessageViewModel>>().To<MessageValidator>();
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return (validatorType == null) ? null : (IValidator)kernel.TryGet(validatorType);
        }
    }
}
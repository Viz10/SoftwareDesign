using AccountService.Application.Commands;
using FluentValidation;
using MediatR;
using Warehouse.Shared.Common;

namespace AccountService.Application.Validations
{
    internal class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator() 
        {
            RuleFor(obj => obj.Email)
              .NotEmpty().WithMessage("Email required!")
              .EmailAddress().WithMessage("Email format is invalid")
              .MaximumLength(100).WithMessage("Email must not exceed 100 characters");

            RuleFor(obj => obj.Password)
                .NotEmpty().WithMessage("Password field cannot be empty!")
                .MaximumLength(255).WithMessage("Password field must not exceed 255 characters");
        }
    }
}

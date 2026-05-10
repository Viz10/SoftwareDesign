using AccountService.Application.Commands;
using FluentValidation;
using System.Text.RegularExpressions;

namespace AccountService.Application.Validations
{
    internal class RegisterAccountCommandValidator : AbstractValidator<RegisterAccountCommand>
    {
        public RegisterAccountCommandValidator()
        {
            RuleFor(obj => obj.Email)
               .NotEmpty().WithMessage("Email required!")
               .EmailAddress().WithMessage("Email format is invalid")
               .MaximumLength(100).WithMessage("Email must not exceed 100 characters");

            RuleFor(obj => obj.Password)
                .NotEmpty().WithMessage("Password field cannot be empty!")
                .MaximumLength(255).WithMessage("Password field must not exceed 255 characters")
                .MinimumLength(5).WithMessage("Password field must have at least 5 characters")
                .Must(NotContainSpaces).WithMessage("Password field must not contain spaces");
                //.Must(RespectPasswordCharacteristics).WithMessage("Password must contain at least: one uppercase letter, one lowercase letter, one digit, and one special character");

            RuleFor(obj => obj.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password field must not be empty!")
                .Equal(obj => obj.Password).WithMessage("Passwords don't match!");
        }
        private bool NotContainSpaces(string field)
        {
            return !string.IsNullOrEmpty(field) && !field.Contains(' ');
        }
        /*private bool RespectPasswordCharacteristics(string password)
        {
            return password.Any(char.IsDigit) && password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(ch => !char.IsLetterOrDigit(ch));
        }
        */
    }
}

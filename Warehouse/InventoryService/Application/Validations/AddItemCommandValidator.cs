using FluentValidation;
using InventoryService.Application.Commands;

namespace InventoryService.Application.Validations
{
    internal class AddItemCommandValidator : AbstractValidator<AddItemCommand>
    {
        public AddItemCommandValidator()
        {
            RuleFor(obj => obj.Name)
              .NotEmpty().WithMessage("Name required!")
              .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

            RuleFor(obj => obj.Description).MaximumLength(500);

            RuleFor(obj => obj.ReferencePricePerItem).GreaterThan(0).LessThan(decimal.MaxValue).WithMessage("Price not in normal range")
                .NotEmpty().WithMessage("Price is required");
        }
    }
}

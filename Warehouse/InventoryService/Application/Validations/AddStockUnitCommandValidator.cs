using FluentValidation;
using InventoryService.Application.Commands;

namespace InventoryService.Application.Validations
{
    internal class AddStockUnitCommandValidator : AbstractValidator<AddStockUnitCommand>
    {
        public AddStockUnitCommandValidator()
        {
            RuleFor(obj => obj.CurrentPrice)
              .NotEmpty().WithMessage("Current Price required!")
              .GreaterThan(0).LessThan(decimal.MaxValue).WithMessage("Price not in normal range");

            RuleFor(obj => obj.Note).MaximumLength(500);

            RuleFor(obj => obj.Quantity)
                .NotEmpty().WithMessage("Quantity required!")
                .GreaterThan(1).LessThan(50).WithMessage("Quantity not in normal range");
        }
    }
}

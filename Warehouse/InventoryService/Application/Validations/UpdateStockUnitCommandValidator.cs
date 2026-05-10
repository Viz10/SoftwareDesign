using FluentValidation;
using InventoryService.Application.Commands;

namespace InventoryService.Application.Validations
{
    internal class UpdateStockUnitCommandValidator : AbstractValidator<UpdateStockUnitCommand>
    {
        public UpdateStockUnitCommandValidator()
        {
            RuleFor(obj => obj.CurrentPrice)
             .NotEmpty().WithMessage("Current Price required!")
             .GreaterThan(0).LessThan(decimal.MaxValue).WithMessage("Price not in normal range");

            RuleFor(obj => obj.Note).MaximumLength(500);
        }
    }
}

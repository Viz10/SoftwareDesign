
using Warehouse.Shared.Enums;

namespace Warehouse.Shared.DTOs.AccountDTO
{
    public record RegisterRequest
    {
        public string Email { get; set; } = string.Empty; /// non positional record , in order to bind values to UI
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public AccountType AccountType { get; set; } = AccountType.Customer;
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Shared.Enums;

namespace Warehouse.Shared.DTOs.AccountDTO
{
    public record RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public AccountType AccountType { get; set; } = AccountType.Customer;
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Shared.Enums;

namespace Warehouse.Shared.DTOs.AccountDTO
{
    public record RegisterRequest(
    string Email,
    string Password,
    string ConfirmPassword,
    AccountType AccountType
    );
}

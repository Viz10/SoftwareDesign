using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.DTOs.AccountDTO
{
    public record LoginRequest(
        string Email,
        string Password
    );
}

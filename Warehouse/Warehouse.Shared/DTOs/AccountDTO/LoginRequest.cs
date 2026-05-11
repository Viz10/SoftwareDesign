using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.DTOs.AccountDTO
{
    public record LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

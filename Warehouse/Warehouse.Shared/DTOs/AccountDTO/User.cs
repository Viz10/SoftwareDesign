using System;
using System.Collections.Generic;
using System.Text;
using Warehouse.Shared.Enums;

namespace Warehouse.Shared.DTOs.AccountDTO
{
    public record User(int Id,string Email,AccountType AccountType);
}

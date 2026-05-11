using System;
using System.Collections.Generic;
using System.Text;

namespace Warehouse.Shared.DTOs.AccountDTO
{
    public record SaveAccountEmailRequest(string AccountEmail,string Content);
}

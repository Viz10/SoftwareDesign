
using Warehouse.Shared.Enums;

namespace Warehouse.Shared.DTOs.AccountDTO
{
    public record User(int Id,string Email,AccountType AccountType);
}

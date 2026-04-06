using System.ComponentModel.DataAnnotations;
using Warehouse.Data.Entities;

namespace Warehouse.Data.DTOs.AccountDTOs
{
    public class AccountViewDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;

        public DateTimeOffset CreatedAt { get; set; } 
        public DateTimeOffset ModifiedTime { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }

        public bool IsActive { get; set; } 
        public bool IsDeleted { get; set; } 

        public AccountType AccountType { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using Warehouse.Data.Entities;

namespace Warehouse.Data.DTOs.AccountDTOs
{
    public class AccountUpdateDTO
    {
        
        [EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        public DateTimeOffset ModifiedTime { get; set; } = DateTimeOffset.UtcNow;

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [Required]
        public AccountType AccountType { get; set; }
    }
}

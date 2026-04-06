using System.ComponentModel.DataAnnotations;
using Warehouse.Data.Entities;

namespace Warehouse.Data.DTOs.AccountDTOs
{
    public class AccountCreateDTO
    {
        [MaxLength(255)]
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;

        [EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        public AccountType AccountType { get; set; }
    }
}

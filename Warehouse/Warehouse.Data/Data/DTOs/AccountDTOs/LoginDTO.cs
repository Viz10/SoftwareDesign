using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Data.DTOs.AccountDTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = null!;

    }
}

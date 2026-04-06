using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.DTOs.AccountDTOs
{
    public class LoginDTO
    {

        [EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;
        
        [MaxLength(255)]
        public string Password { get; set; } = null!;

    }
}

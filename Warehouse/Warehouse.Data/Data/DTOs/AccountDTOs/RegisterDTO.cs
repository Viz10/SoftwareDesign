using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Warehouse.Data.Entities;

namespace Warehouse.Data.Data.DTOs.AccountDTOs
{
    public class RegisterDTO
    {
        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(255)]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        [EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        public AccountType AccountType { get; set; }
    }
}

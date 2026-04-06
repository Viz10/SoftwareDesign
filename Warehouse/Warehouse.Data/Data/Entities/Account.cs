using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Warehouse.Data.Entities
{
    public enum AccountType
    {
        Customer = 1,
        Seller = 2,
        Admin = 3,
    }
    public class Account : IEntity
    {
        [Key]
        public int Id { get; set; }


        [MaxLength(255)]
        public string PasswordHashed { get; set; } = null!;

        [EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;

        
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset ModifiedTime { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }


        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;

        [Required]
        public AccountType AccountType { get; set; }
    }

}

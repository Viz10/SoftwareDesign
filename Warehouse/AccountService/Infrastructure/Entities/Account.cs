using System.ComponentModel.DataAnnotations;
using Warehouse.Shared.Common;
using Warehouse.Shared.Enums;

namespace AccountService.Infrastructure.Entities
{
    public class Account : ISoftDeletable
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string PasswordHashed { get; set; } = null!;

        [EmailAddress, MaxLength(100)]
        public string Email { get; set; } = null!;


        public DateTimeOffset CreatedAtTime { get; set; }
        public DateTimeOffset? LastModifiedTime { get; set; }
        public bool IsDeleted { get; set; } = false;


        [Required]
        public AccountType AccountType { get; set; }

        public List<AccountEmail> AccountEmails { get; set; } = new List<AccountEmail>();
    }
}

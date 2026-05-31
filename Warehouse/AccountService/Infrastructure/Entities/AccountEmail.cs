using System.ComponentModel.DataAnnotations;

namespace AccountService.Infrastructure.Entities
{
    public class AccountEmail
    {
        [Key]
        public int Id { get; set; }

        public int AccountId { get; set; } // FK
        public Account Account { get; set; } = null!;
        
        public string EmailContent { get; set; } = null!;
    }
}

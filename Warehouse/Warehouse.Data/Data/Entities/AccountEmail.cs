using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Warehouse.Data.Entities;

namespace Warehouse.Data.Data.Entities
{
    public class AccountEmail
    {
        [Key]
        public int Id { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;
        public string EmailContent { get; set; } = null!;

    }
}

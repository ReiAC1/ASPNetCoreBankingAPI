using BankingApp.Users.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.AccountTypes
{
    public class AccountType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        // DefaultValue allows us to specifcy the initial value of a Property
        [DefaultValue(true)]
        public bool IsAvailable { get; set; }
    }
}

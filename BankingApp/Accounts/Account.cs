using BankingApp.AccountOwners;
using BankingApp.AccountTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Accounts
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public AccountType AccountType { get; set; }
        public float Balance { get; set; }
        public string Nickname { get; set; }
        public bool IsActive { get; set; } = true;


        public ICollection<AccountOwner> Owners { get; set; }
    }
}

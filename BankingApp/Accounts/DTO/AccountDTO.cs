using BankingApp.AccountTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Accounts.DTO
{
    public class AccountDTO
    {
        public int ID { get; set; }
        public AccountType AccountType { get; set; }
        public float Balance { get; set; }
        public string Nickname { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

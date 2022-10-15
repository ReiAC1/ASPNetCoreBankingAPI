using BankingApp.AccountTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Accounts.DTO
{
    public class AccountRequest
    {
        public int ID { get; set; }
        public int AccountType { get; set; }
        public string Nickname { get; set; }
    }
}

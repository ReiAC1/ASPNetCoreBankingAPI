using BankingApp.AccountTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Accounts.DTO
{
    public class AccountResponse
    {
        public int ID { get; set; }
        public AccountType AccountType { get; set; }
        public float Balance { get; set; }
        public string Nickname { get; set; }
        public bool IsActive { get; set; }

        public int[] AccountOwners { get; set; }

        public AccountResponse(Account ac)
        {
            ID = ac.ID;
            AccountType = ac.AccountType;
            Balance = ac.Balance;
            Nickname = ac.Nickname;

            if (ac.Owners == null)
            {
                AccountOwners = new int[0];
                return;
            }

            AccountOwners = new int[ac.Owners.Count];

            int i = 0;
            foreach (var owner in ac.Owners)
            {
                AccountOwners[i++] = owner.ID;
            }
        }
    }
}

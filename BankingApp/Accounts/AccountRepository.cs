using BankingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Accounts
{
    public class AccountRepository : GenericRepository<Account>
    {
        public AccountRepository(BankContext context)
            : base(context)
        {

        }
    }
}

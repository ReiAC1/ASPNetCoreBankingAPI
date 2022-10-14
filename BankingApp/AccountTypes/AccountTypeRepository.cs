using BankingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.AccountTypes
{
    public class AccountTypeRepository : GenericRepository<AccountType>
    {
        public AccountTypeRepository(BankContext context)
            : base(context)
        {

        }
    }
}

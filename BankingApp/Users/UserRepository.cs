using BankingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Users
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(BankContext context)
            : base(context)
        {

        }
    }
}

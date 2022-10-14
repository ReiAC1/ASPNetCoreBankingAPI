using BankingApp.AccountOwners;
using BankingApp.Accounts;
using BankingApp.AccountTypes;
using BankingApp.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions<BankContext> context)
            : base(context)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<AccountType>().ToTable("account_types");
            modelBuilder.Entity<AccountOwner>().ToTable("account_owners");
            modelBuilder.Entity<Account>().ToTable("accounts");
        }
    }
}

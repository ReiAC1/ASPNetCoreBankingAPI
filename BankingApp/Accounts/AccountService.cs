using BankingApp.Accounts.DTO;
using BankingApp.AccountTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Accounts
{
    public class AccountService
    {
        AccountRepository _repository;
        AccountTypeService _atService;

        public AccountService(BankContext context)
        {
            _repository = new AccountRepository(context);
            _atService = new AccountTypeService(context);
        }

        public Account GetAccountById(int id)
        {
            // if ID is invalid, return null
            if (id <= 0)
                return null;

            return _repository.GetById(id);
        }

        public AccountResponse GetById(int id)
        {
            var ac = GetAccountById(id);

            if (ac == null) return null;

            return new AccountResponse(ac);
        }

        public IEnumerable<AccountResponse> GetAll()
        {
            // gets all users, but returns it as an account response
            return _repository.GetAll().Select(e => new AccountResponse(e));
        }

        public Account SaveOrUpdate(AccountRequest request)
        {

            Account entity = GetAccountById(request.ID);

            if (entity != null)
            {
                // the only thing that should manually be updated is the nickname
                // everything else should be handled by another service
                if (request.Nickname != null && request.Nickname.Trim().Length > 0)
                    entity.Nickname = request.Nickname;
            }
            else
            {
                entity = new Account();
                entity.Nickname = request.Nickname;
                entity.AccountType = _atService.GetById(request.AccountType);
                entity.Balance = 0;
                entity.ID = 0;

                // fail if we cannot use this account type for new accounts
                if (!entity.AccountType.IsAvailable)
                    return null;
            }

            if (!Verify(entity))
                return null;

            // add the user to the database
            _repository.Add(entity);

            // return a new response
            return entity;
        }

        public Account Delete(int id)
        {
            // ensure that the user exists before deleting
            var entity = GetAccountById(id);

            if (entity == null)
                return null; // if user does not exist, just return

            // instead of deleting, we just set is active to false
            entity.IsActive = false;

            _repository.Add(entity);
            return entity;
        }

        public bool PerformTransaction(int id, float amount)
        {
            Account ac = GetAccountById(id);

            if (ac == null || ac.Balance + amount < 0)
                return false;

            ac.Balance += amount;
            _repository.Add(ac);

            return true;
        }

        bool Verify(Account Account)
        {
            // negative ID values for email shouldn't exist. 0 indicates a new account. 1+ indicates an existing account
            if (Account.ID < 0) return false;

            // checks to see if name is valid
            if (Account.Nickname == null || Account.Nickname.Trim().Length == 0) return false;

            // return true if none of the checks failed
            return true;
        }
    }
}

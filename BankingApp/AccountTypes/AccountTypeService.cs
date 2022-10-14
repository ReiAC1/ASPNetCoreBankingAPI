using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.AccountTypes
{
    public class AccountTypeService
    {
        AccountTypeRepository _repository;

        public AccountTypeService(BankContext context)
        {
            _repository = new AccountTypeRepository(context);
        }

        public AccountType GetById(int id)
        {
            // if ID is invalid, return null
            if (id <= 0)
                return null;

            return _repository.GetById(id);
        }

        public IEnumerable<AccountType> GetAll()
        {
            // gets all users, but returns it as a user response
            return _repository.GetAll();
        }


        public AccountType SaveOrUpdate(AccountType request)
        {

            AccountType entity = GetById(request.ID);

            if (entity != null)
            {
                // perform request updates by checking and updating each field individually

                if (request.Name != null && request.Name.Trim().Length > 0)
                    entity.Name = request.Name;

                if (request.Description != null && request.Description.Trim().Length > 0)
                    entity.Description = request.Description;
            }
            else
            {
                entity = request;
            }

            if (!Verify(entity))
                return null;

            // add the user to the database
            _repository.Add(entity);

            // return a new response
            return entity;
        }

        public AccountType Delete(int id)
        {
            // ensure that the user exists before deleting
            var entity = GetById(id);

            if (entity == null)
                return null; // if user does not exist, just return

            // instead of deleting, we just set is active to false
            entity.IsAvailable = false;

            return SaveOrUpdate(entity);
        }

        bool Verify(AccountType accountType)
        {
            // negative ID values for email shouldn't exist. 0 indicates a new account. 1+ indicates an existing account
            if (accountType.ID < 0) return false;

            // checks to see if name/description are null or empty
            if (accountType.Name == null || accountType.Name.Trim().Length == 0) return false;
            if (accountType.Description == null || accountType.Description.Trim().Length == 0) return false;

            // return true if none of the checks failed
            return true;
        }
    }
}

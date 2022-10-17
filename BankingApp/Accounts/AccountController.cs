using BankingApp.Accounts.DTO;
using BankingApp.Authentication;
using BankingApp.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Accounts
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : AuthorizedControllerBase
    {
        AccountService _aService;
        UserService _userService;

        public AccountController(BankContext context)
        {
            _aService = new AccountService(context);
            _userService = new UserService(context);
        }


        [HttpGet("{id?}")]
        public IEnumerable<AccountResponse> Get(int? id)
        {
            // returns all users
            if (id == null)
            {
                return _aService.GetAll();
            }

            // returns selected account, or empty array if nothing
            var a = _aService.GetById(id.Value);
            if (a == null || a.ID == 0)
            {
                return new AccountResponse[] { };
            }

            // return the requested account's info
            return new AccountResponse[] { a };
        }


        [HttpPost("create")]
        public bool CreateAccount([FromBody] AccountRequest request)
        {
            User authUser = _userService.GetUserById(AuthUserID);

            if (authUser == null)
                return false;

            Account a = _aService.SaveOrUpdate(request);

            if (a == null)
                return false;

            AccountOwners.AccountOwner owner = new AccountOwners.AccountOwner();
            owner.Account = a;
            owner.User = authUser;

            a.Owners.Add(owner);

            return _aService.Persist(a) != null;
        }
    }
}

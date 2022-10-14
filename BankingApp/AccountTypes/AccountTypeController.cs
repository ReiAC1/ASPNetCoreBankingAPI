using BankingApp.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.AccountTypes
{

    [ApiController]
    [Route("[controller]")]
    public class AccountTypeController : AuthorizedControllerBase
    {

        AccountTypeService _accountTypeService;

        public AccountTypeController(BankContext context)
        {
            _accountTypeService = new AccountTypeService(context);
        }

        [HttpGet("{id?}")]
        public IEnumerable<AccountType> Get(int? id)
        {
            // returns all users
            if (id == null)
            {
                return _accountTypeService.GetAll();
            }

            // returns selected user, or empty arrasy if nothing
            var atype = _accountTypeService.GetById(id.Value);
            if (atype == null || atype.ID == 0)
            {
                return new AccountType[] { };
            }

            // return the requested user's info
            return new AccountType[] { atype };
        }

        [HttpPost("create")]
        public bool Register([FromBody] AccountType request)
        {
            if (!AuthUserAdmin)
            {
                Response.StatusCode = Unauthorized().StatusCode;
                return false;
            }

            return _accountTypeService.SaveOrUpdate(request) != null;
        }

        [HttpPut("")]
        public bool Update([FromBody] AccountType request)
        {
            if (!AuthUserAdmin)
            {
                Response.StatusCode = Unauthorized().StatusCode;
                return false;
            }

            return _accountTypeService.SaveOrUpdate(request) != null;
        }

        [HttpDelete("delete/{id}")]
        public bool Delete(int id)
        {
            if (!AuthUserAdmin)
            {
                Response.StatusCode = Unauthorized().StatusCode;
                return false;
            }

            return _accountTypeService.Delete(id) != null;
        }
    }
}

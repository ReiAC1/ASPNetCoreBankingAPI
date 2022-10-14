using BankingApp.Authentication;
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
    }
}

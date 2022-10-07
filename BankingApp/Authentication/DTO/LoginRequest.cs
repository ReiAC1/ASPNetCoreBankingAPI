using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Authentication.DTO
{
    public class LoginRequest
    {
        public String Email { get; set; }
        public String Password { get; set; }
    }
}

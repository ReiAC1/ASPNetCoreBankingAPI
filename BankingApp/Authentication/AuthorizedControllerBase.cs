using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankingApp.Authentication
{
    public abstract class AuthorizedControllerBase : ControllerBase
    {
        public int AuthUserID
        {
            get
            {
                if (!HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    return -1;
                }

                string jwt = HttpContext.Request.Headers["Authorization"];

                var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);

                return int.Parse(token.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name)).Value);
            }
        }

        public bool AuthUserAdmin
        {
            get
            {
                if (!HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    return false;
                }

                string jwt = HttpContext.Request.Headers["Authorization"];

                var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);

                return bool.Parse(token.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role)).Value);
            }
        }
    }
}

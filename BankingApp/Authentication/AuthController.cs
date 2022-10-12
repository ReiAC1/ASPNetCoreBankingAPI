using BankingApp.Authentication.DTO;
using BankingApp.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Authentication
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        readonly IConfiguration _config;
        readonly UserService _userService;

        const double EXPIRY_DURATION_MINUTES = 60 * 24 * 7;

        public AuthController(IConfiguration config, BankContext context)
        {
            _config = config;
            _userService = new UserService(context);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            IActionResult response = Unauthorized();
            var user = _userService.Login(login.Email, login.Password);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }


        string GenerateJSONWebToken(User userInfo)
        {
            var claims = new []
            {
                new Claim(ClaimTypes.Name, userInfo.ID.ToString()),
                new Claim(ClaimTypes.Email, userInfo.Email),
                new Claim(ClaimTypes.Role, userInfo.IsAdmin.ToString()),
                new Claim(ClaimTypes.NameIdentifier,
            Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"], claims,
                expires: DateTime.Now.AddMinutes(EXPIRY_DURATION_MINUTES), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}

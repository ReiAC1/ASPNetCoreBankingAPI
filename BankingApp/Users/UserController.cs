using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankingApp.Users;
using BankingApp.Users.DTO;
using Microsoft.AspNetCore.Authorization;
using BankingApp.Authentication;

namespace BankingApp.Users
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : AuthorizedControllerBase
    {
        private readonly UserService _userService;

        public UserController(BankContext bankContext)
        {
            _userService = new UserService(bankContext);
        }

        [HttpGet("{id?}")]
        public IEnumerable<UserResponse> Get(int? id)
        {
            // returns all users
            if (id == null)
            {
                return _userService.GetAll();
            }

            // returns selected user, or empty arrasy if nothing
            var user = _userService.GetById(id.Value);
            if (user == null || user.ID == 0)
            {
                return new UserResponse[] { };
            }

            // return the requested user's info
            return new UserResponse[] { user };
        }

        [HttpPost("create")]
        public bool Register([FromBody] UserRequest request)
        {
            return _userService.SaveOrUpdate(request) != null;
        }

        [HttpPut("")]
        public bool Update([FromBody] UserRequest request)
        {
            User authUser = _userService.GetUserById(AuthUserID);

            if (authUser == null)
                return false;

            request.ID = authUser.ID;

            return _userService.SaveOrUpdate(request) != null;
        }

        [HttpDelete("deactivate")]
        public bool Delete()
        {
            User authUser = _userService.GetUserById(AuthUserID);

            if (authUser == null)
                return false;

            return _userService.Delete(AuthUserID) == null;
        }
    }
}

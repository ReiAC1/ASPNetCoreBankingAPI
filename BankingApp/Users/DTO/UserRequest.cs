using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Users.DTO
{
    public class UserRequest
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }

        public UserRequest() { }

        public UserRequest(User user)
        {
            ID = user.ID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Password = user.Password;
            RegistrationDate = user.RegistrationDate;
        }
    }
}

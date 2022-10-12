using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Users.DTO
{
    public class UserResponse
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }


        public UserResponse() { }
        public UserResponse(User user)
        {
            if (user == null) return;

            ID = user.ID;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            RegistrationDate = user.RegistrationDate;
        }
    }
}

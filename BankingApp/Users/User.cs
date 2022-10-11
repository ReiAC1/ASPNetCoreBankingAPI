using BankingApp.Users.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BankingApp.Users
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime RegistrationDate { get; set; }

        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

        public User () { IsActive = true; IsAdmin = true; }

        public User (UserRequest request)
        {
            ID = request.ID;
            FirstName = request.FirstName;
            LastName = request.LastName;
            Email = request.Email;
            Password = request.Password;
            RegistrationDate = request.RegistrationDate;
            IsAdmin = true;
            IsActive = true;
        }
    }
}

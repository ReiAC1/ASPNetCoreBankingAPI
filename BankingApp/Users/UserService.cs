using BankingApp.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankingApp.Users
{
    public class UserService
    {
        UserRepository _repository;

        public UserService(BankContext context)
        {
            _repository = new UserRepository(context);
        }

        public User Login(string email, string password)
        {
            password = EncryptPassword(password);
            return GetAllUsers().FirstOrDefault(u =>
            {
                return u.Email.Equals(email) && u.Password.Equals(password);
            });
        }

        public UserResponse GetById(int id)
        {
            if (id < 0)
                return null;

            return new UserResponse(_repository.GetById(id));
        }

        public User GetUserById(int id)
        {
            if (id < 0)
                return null;

            return _repository.GetById(id);
        }

        public IEnumerable<UserResponse> GetAll()
        {
            // gets all users, but returns it as a user response
            return GetAllUsers().Select(u => new UserResponse(u));
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _repository.GetAll();
        }

        public UserResponse SaveOrUpdate(UserRequest request)
        {
            // encrypt password
            request.Password = EncryptPassword(request.Password);
            request.ConfirmPassword = EncryptPassword(request.ConfirmPassword);

            User entity = GetUserById(request.ID);

            if (entity == null)
            {
                // create new user if entity does not exist in database

                entity = new User(request);
                entity.RegistrationDate = DateTime.Now;
            }
            else
            {
                // perform request updates

                if (request.FirstName != null && request.FirstName.Trim().Length > 0)
                    entity.FirstName = request.FirstName;

                if (request.LastName != null && request.LastName.Trim().Length > 0)
                    entity.LastName = request.LastName;

                if (request.Email != null && request.Email.Trim().Length > 0)
                    entity.Email = request.Email;

                if (request.Password != null && request.Password.Trim().Length > 0 && request.ConfirmPassword.Equals(request.Password))
                    entity.Password = request.Password;
            }

            // validation of request 
            if (!Verify(entity) || !request.Password.Equals(request.ConfirmPassword))
                return null;

            _repository.Add(entity);

            return new UserResponse(entity);
        }

        public UserResponse Delete(int id)
        {
            User entity = GetUserById(id);

            if (entity == null)
                return null;

            entity.IsActive = false;

            return SaveOrUpdate(new UserRequest(entity));
        }

        bool Verify(User user)
        {
            Regex emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            if (user.ID < 0) return false;
            if (user.FirstName == null || user.FirstName.Trim().Length == 0) return false;
            if (user.LastName == null || user.LastName.Trim().Length == 0) return false;

            if (user.RegistrationDate.Ticks > DateTime.Now.Ticks) return false;

            if (!emailRegex.IsMatch(user.Email)) return false;
            if (user.Password.Length != 64) return false;

            return true;
        }

        string EncryptPassword(string pass)
        {
            // confirm our password is a valid regex match before encrypting
            Regex passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            if (!passwordRegex.IsMatch(pass))
                return ""; // otherwise return false, which ensures the password will never succeed


            // After confirming password validity, we can now begin converting the password into a SHA256 string
            SHA256CryptoServiceProvider sha256 = new SHA256CryptoServiceProvider();

            // convert the string into an array of bytes
            byte[] ba1 = System.Text.Encoding.UTF8.GetBytes(pass);

            // hash using the above crypto service provider
            ba1 = sha256.ComputeHash(ba1);

            // then rebuild the string based off the computed byte array
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            foreach (byte b in ba1)
            {
                sb.Append(b.ToString("x2").ToLower());
            }

            // return the encrypted password
            return sb.ToString();
        }
    }
}

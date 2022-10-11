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

            // ensure our email and password are met as well as that the account is active
            return GetAllUsers().FirstOrDefault(u =>
            {
                return u.Email.Equals(email) && u.Password.Equals(password) && u.IsActive;
            });
        }

        public UserResponse GetById(int id)
        {
            // if ID is invalid, return null
            if (id <= 0)
                return null;

            return new UserResponse(_repository.GetById(id));
        }

        public User GetUserById(int id)
        {
            // if ID is invalid, return null
            if (id <= 0)
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
            // encrypt password and password confirmation
            request.Password = EncryptPassword(request.Password);
            request.ConfirmPassword = EncryptPassword(request.ConfirmPassword);

            User entity = GetUserById(request.ID);

            if (entity == null)
            {
                // create new user if entity does not exist in database

                entity = new User(request);
                entity.RegistrationDate = DateTime.Now; // registration should always be the current time
            }
            else
            {
                // perform request updates by checking and updating each field individually

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

            // add the user to the database
            _repository.Add(entity);

            // return a new response
            return new UserResponse(entity);
        }

        public UserResponse Delete(int id)
        {
            // ensure that the user exists before deleting
            User entity = GetUserById(id);

            if (entity == null)
                return null; // if user does not exist, just return

            // instead of deleting, we just set is active to false
            entity.IsActive = false;

            return SaveOrUpdate(new UserRequest(entity));
        }

        bool Verify(User user)
        {
            // email regex that says:
            // [\w-\.]+                 1 or more occurences of \w (word characters), - (dashes), or . (periods)
            // @                        literally the @ sign
            // ([\w-]+\.)+              1 or more occurences of the next line followed by a . (period)
            // [\w-]+                   1 or more occurences of \w (word characters) or - (dashes)
            // [\w-]{2,4}               2 - 4 occurences of \w (word characters) or - (dashes)
            Regex emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");


            // negative ID values for email shouldn't exist. 0 indicates a new account. 1+ indicates an existing account
            if (user.ID < 0) return false;

            // checks to see if first/last names are null or empty
            if (user.FirstName == null || user.FirstName.Trim().Length == 0) return false;
            if (user.LastName == null || user.LastName.Trim().Length == 0) return false;

            // ensure we're not registering in the future
            if (user.RegistrationDate.Ticks > DateTime.Now.Ticks) return false;

            // ensure the email matches the above regex
            if (!emailRegex.IsMatch(user.Email)) return false;

            // ensure our password is a proper base64 string for SHA256
            if (user.Password.Length != 64) return false;

            // return true if none of the checks failed
            return true;
        }

        string EncryptPassword(string pass)
        {

            // password regex that says:
            // (?=)                         do not store this group as a variable
            // .*                           0 or more instances of any non-whitespace character
            //
            // Using the above you can then determine each segment by adding the following:
            // [a-z]                        contains any lowercase letter AND
            // [A-Z]                        contains any uppercase letter AND
            // \d                           contains any digit            AND
            // [@$!%*?&]                    contains any symbol
            // {8,}                         at least 8 characters

            Regex passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");


            // confirm our password is a valid regex match before encrypting
            if (!passwordRegex.IsMatch(pass))
                return ""; // otherwise return an empty string


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

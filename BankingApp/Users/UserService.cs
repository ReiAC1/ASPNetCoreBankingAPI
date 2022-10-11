using BankingApp.Users.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (!Verify(entity) || request.Password != request.ConfirmPassword)
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
            Regex passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            Regex emailRegex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            if (user.ID < 0) return false;
            if (user.FirstName == null || user.FirstName.Trim().Length == 0) return false;
            if (user.LastName == null || user.LastName.Trim().Length == 0) return false;

            if (user.RegistrationDate.Ticks > DateTime.Now.Ticks) return false;

            if (!emailRegex.IsMatch(user.Email)) return false;
            if (!passwordRegex.IsMatch(user.Password)) return false;

            return true;
        }
    }
}


using aton_intern.DTOs;
using Aton_intern.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aton_intern.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new List<User>();

        private readonly object _lock = new object();

        //create/add a user to the list
        public string CreateUser(User user)
        {
            lock (_lock)
            {
                var getUser = IsUserExists(user.Username);

                if (IsUserExists(user.Username))
                {
                    throw new InvalidOperationException("Login already exists.");
                }

                user.Id = Guid.NewGuid();
                user.CreatedOn = DateTime.UtcNow;
                user.CreatedBy = "";

                _users.Add(user);

                return "user created";
            }
        }

        //retrieve all users regardless of their status
        public IEnumerable<UserListDto> GetAllUsers()
        {
            lock (_lock)
            {
                return _users.Select(u => new UserListDto { UserName = u.Username }).ToList();
            }
        }


        // get if the user exist in the list
        public bool IsUserExists(string username)
        {
            var findUser = _users.Any(x => x.Username == username);

            return findUser;
        }

        // soft removal of user
        public string SoftDelete(string username)
        {
            var getUserToRemove = GetUserByUsername(username);

            getUserToRemove.RevokedBy = "Administrator";
            getUserToRemove.RevokedOn = DateTime.UtcNow;

            throw new NotImplementedException();
        }

        //total removal of user
        public string HardDelete(string username)
        {
            var getUserToRemove = GetUserByUsername(username);

            var removeUser = _users.Remove(getUserToRemove);

            return "user removed successfully";

        }


        //retrieve user by its username
        public User GetUserByUsername(string username)
        {
            var user = _users.SingleOrDefault(u => u.Username == username);

            if (user == null) throw new Exception("User does not exist");

            return user;
        }

        //update user
        public string UpdateUser(UpdateCredentials credentials, string username)
        {
            var user = GetUserByUsername(username);
            if (!string.IsNullOrWhiteSpace(credentials.Username))
            {
                if (IsUserExists(credentials.Username)) user.Username = credentials.Username;
            }

            if (!string.IsNullOrWhiteSpace(credentials.Password)) user.Password = credentials.Password;

            if(!string.IsNullOrWhiteSpace(credentials.Name)) user.Name = credentials.Name;

            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = "user";
            throw new NotImplementedException();
        }

        public string ChangePassword(string username, [FromBody] string password)
        {
            throw new NotImplementedException();
        }

        //get all active user
        public IEnumerable<UserListDto> GetAllActiveUsers()
        {
            return _users.Where(ua => ua.IsActive).Select(u => new UserListDto { UserName = u.Username}).ToList();
        }

        public IEnumerable<UserListDto> GetUserOfCertainAge()
        {
            throw new NotImplementedException();
        }

        public string UserRecovery(string username)
        {
            var userToRetrieve = _users.SingleOrDefault(u => u.Username == username && u.RevokedOn != null);

           if (userToRetrieve == null)
           {
                throw new InvalidOperationException("user does not exist");
           }

           userToRetrieve.RevokedOn = null; userToRetrieve.RevokedBy = null;

            return "User has been recovery";
        }
    }
}

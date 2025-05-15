
using aton_intern.DTOs;
using Aton_intern.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Aton_intern.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly List<User> _users = new List<User>();

        private readonly object _lock = new object();

        public UserService()
        {
            
        }

        //create/add a user to the list
        public string CreateUser(CreateUserDto user, string createdBy)
        {
            lock (_lock)
            {
                var getUser = IsUserExists(user.Username);

                if (IsUserExists(user.Username))
                {
                    throw new InvalidOperationException("Username already exists.");
                }

                var createUser = new User
                {
                    Id = Guid.NewGuid(),
                    Username = user.Username,
                    Name = user.Name,
                    Password = user.Password,
                    Gender = user.Gender,
                    CreatedBy = createdBy,
                    CreatedOn = DateTime.UtcNow,
                    BirthDate = user.BirthDate,
                    IsAdmin = user.IsAdmin,
                };

                _users.Add(createUser);

                return "user created";
            }
        }

        //retrieve all users regardless of their status
        public IEnumerable<UserListDto> GetAllUsers()
        {
            lock (_lock)
            {
                return _users
                    .Select(u => new UserListDto { 
                        UserName = u.Username, 
                        Name = u.Name,
                        Gender = u.Gender,
                        Birthday = u.BirthDate,
                        IsActive = u.IsActive 
                    })
                    .ToList();
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
        public string UpdateUser(UpdateCredentialsDto credentials, string username, string modifiedBy)
        {
            var user = GetUserByUsername(username);


            if(!string.IsNullOrWhiteSpace(credentials.Name)) user.Name = credentials.Name;

            if(credentials.Gender.HasValue) user.Gender = credentials.Gender.Value;

            if (credentials.BirthDate != null) user.BirthDate = (DateTime)credentials.BirthDate;

            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = modifiedBy;

            return "Credientials updated";
        }


        // change password method
        public string ChangePassword(string username, [FromBody] string newPassword, string modifiedBy)
        {
            var getUser = GetUserByUsername(username);

            getUser.Password = newPassword;
            getUser.ModifiedOn = DateTime.UtcNow;
            getUser.ModifiedBy = modifiedBy;

            return "password changed";
        }


        //get all active user
        public IEnumerable<UserListDto> GetAllActiveUsers()
        {
            return _users.Where(ua => ua.IsActive)
                .OrderBy(c => c.CreatedOn)
                .Select(u => new UserListDto { 
                    UserName = u.Username,
                    Name = u.Name,
                    Gender = u.Gender,
                    Birthday = u.BirthDate
                })
                .ToList();
        }


        //user over a certain age range which return age and above
        public IEnumerable<UserListDto> GetUsersOfCertainAge(int ageRange)
        {
            var getUsersAge = _users
               .Where(ua => ua.BirthDate.Year >= DateTime.UtcNow.AddYears(-ageRange).Year)
               .Select(u => new UserListDto { UserName = u.Username })
               .ToList();

            return getUsersAge;
        }

        //account recovery
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

        //update username
        public string ChangeUsername(string username, string newUsername, string modifiedBy)
        {
            var user = GetUserByUsername(username);
            if (!string.IsNullOrWhiteSpace(newUsername))
            {
                var query = (!IsUserExists(newUsername))
                    ? user.Username = newUsername
                    : throw new Exception("User exists in the system");
            }

            user.ModifiedOn = DateTime.UtcNow;
            user.ModifiedBy = modifiedBy;

            return "username updated";
        }

        //retrieving user credientials
        public Credientials GetCreentials(string username)
        {
            var user = GetUserByUsername(username);

            if (user == null || user.Username != username)
            {
                throw new Exception("Invalid user: validate your credientials");
            }

            return new Credientials { Username = user.Username, Password = user.Password };
        }
    }
}

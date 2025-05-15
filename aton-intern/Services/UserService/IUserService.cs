using aton_intern.DTOs;
using Aton_intern.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aton_intern.Services.UserService
{
    public interface IUserService
    {
        string CreateUser(CreateUserDto user, string createdBy);

        bool IsUserExists(string username);

        User GetUserByUsername(string username);

        string UpdateUser(UpdateCredentialsDto credentials, string username, string modifiedBy);

        string ChangeUsername(string username, string newUserName, string modifiedBy);

        string ChangePassword(string username, [FromBody]string password, string modifiedBy);

        IEnumerable<UserListDto> GetAllUsers();

        IEnumerable<UserListDto> GetAllActiveUsers();

        Credientials GetCreentials(string username);

        IEnumerable<UserListDto> GetUsersOfCertainAge(int ageRange);

        string SoftDelete(string username);

        string HardDelete(string username);

        string UserRecovery(string username);
    }
}

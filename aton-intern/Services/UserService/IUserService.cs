using aton_intern.DTOs;
using Aton_intern.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aton_intern.Services.UserService
{
    public interface IUserService
    {
        string CreateUser(CreateUserDto user);

        //string CreateUser2(CreateUserDto userDto);

        bool IsUserExists(string username);

        User GetUserByUsername(string username);

        string UpdateUser(UpdateCredentials credentials, string username);

        string ChangePassword(string username, [FromBody]string password);

        IEnumerable<UserListDto> GetAllUsers();

        IEnumerable<UserListDto> GetAllActiveUsers();

        IEnumerable<UserListDto> GetUserOfCertainAge();

        string SoftDelete(string username);

        string HardDelete(string username);

        string UserRecovery(string username);
    }
}

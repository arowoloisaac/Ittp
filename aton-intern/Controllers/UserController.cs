using aton_intern.DTOs;
using Aton_intern.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aton_intern.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService service;

        public UserController(IUserService service)
        {
            this.service = service;
        }


        [HttpPost]
        [Authorize(Policy = "IsAdmin")]
        [Route("user/create")]
        public IActionResult CreateUser(CreateUserDto dto)
        {
            var claimedUser = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            if (claimedUser == null)
            {
                return Unauthorized();
            }
            return Ok(service.CreateUser(dto, claimedUser.Value));
        }

        
        [HttpGet]
        [Authorize(Policy = "IsAdmin")]
        [Route("users")]
        public IActionResult GetUsers()
        {
            return Ok(service.GetAllUsers());   
        }

        
        [HttpGet]
        [Authorize(Policy = "IsAdmin")]
        [Route("users/active")]
        public IActionResult GetActiveUsers()
        {
            return Ok(service.GetAllActiveUsers());
        }


        [HttpPut]
        [Authorize(Policy ="IsAdmin")]
        [Route("user/{username}/soft-delete")]
        public IActionResult SoftDeleteUser(string username)
        {
            return Ok(service.SoftDelete(username));
        }


        [HttpDelete]
        [Authorize(Policy ="IsAdmin")]
        [Route("user/{username}/hard-delete")]
        public IActionResult HardDeleteUser(string username)
        {
            return Ok(service.HardDelete(username));
        }


        [HttpPut]
        [Route("user/update-user")]
        public IActionResult UpdateUser(UpdateCredentialsDto dto, string userToUpdate) 
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            var getUser = user != null ? service.GetUserByUsername(user.Value): throw new Exception("Invalid credientials");

            if (getUser.IsAdmin && getUser.IsActive)
            {
                return Ok(service.UpdateUser(dto, userToUpdate, getUser.Username));
            }
            else
            {
                if (user.Value == getUser.Username && getUser.IsActive)
                {
                    return Ok(service.UpdateUser(dto, userToUpdate, getUser.Username));
                }
                return BadRequest("Unable to perform request");
            }
        }


        [HttpPut]
        [Route("user/change-username")]
        public IActionResult ChangeUsername(string username, string newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername))
            {
                throw new InvalidOperationException("Username does not have value");
            }

            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            var getUser = user != null ? service.GetUserByUsername(user.Value) : throw new Exception("Invalid credientials");

            if (getUser.IsAdmin && getUser.IsActive)
            {
                return Ok(service.ChangeUsername(username, newUsername, getUser.Username));
            }
            else
            {
                if (user.Value == getUser.Username && getUser.IsActive)
                {
                    return Ok(service.ChangeUsername(username, newUsername, newUsername));
                }
                return BadRequest("Unable to perform request");
            }
        }


        [HttpPut]
        [Route("user/change-password")]
        public IActionResult ChangePassword(string username, [FromBody]string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                throw new InvalidOperationException("Password does not have value");
            }

            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            var getUser = user != null ? service.GetUserByUsername(user.Value) : throw new Exception("Invalid credientials");

            if (getUser.IsAdmin && getUser.IsActive)
            {
                return Ok(service.ChangeUsername(username, newPassword, getUser.Username));
            }
            else
            {
                if (user.Value == getUser.Username && getUser.IsActive)
                {
                    return Ok(service.ChangePassword(username, newPassword, user.Value));
                }
                return BadRequest("Unable to perform request");
            }
        }


        [HttpGet]
        [Route("user/credientials")]
        public IActionResult UserCrediential()
        {
            var claimUser = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);

            if (claimUser == null)
            {
                return BadRequest("System can not validate empty user");
            }

            return Ok(claimUser.Value);
        }


        [HttpGet]
        [Authorize(Policy ="IsAdmin")]
        [Route("users/age")]
        public IActionResult GetUsersByAge(int age)
        {
            return Ok(service.GetUsersOfCertainAge(age));
        }


        [HttpPut]
        [Authorize(Policy ="IsAdmin")]
        [Route("user/{username}/recovery")]
        public IActionResult UserRecovery(string username)
        {
            return Ok(service.UserRecovery(username));
        }
    }
}

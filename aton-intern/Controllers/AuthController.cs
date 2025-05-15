using aton_intern.DTOs;
using Aton_intern.Services.Auth;
using Aton_intern.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace aton_intern.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService service;
        private readonly TokenService _tokenService;

        public AuthController(IUserService service,TokenService tokenService)
        {
            this.service = service;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginCredientials request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Login and password are required.");

            var user = service.GetUserByUsername(request.Username);
            if (user == null || user.Password != request.Password)
                return Unauthorized("Invalid login or password.");

            if (!user.IsActive)
                return Unauthorized("User is revoked.");

            var token = _tokenService.GenerateToken(user);
            return Ok(new { token });
        }
    }
}

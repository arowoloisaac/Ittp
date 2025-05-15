using aton_intern.DTOs;
using Aton_intern.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aton_intern.Controllers
{
    [Route("api/[controller]")]
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
        [Route("user/create")]
        [Authorize(Policy ="IsAdmin")]
        public IActionResult CreateUser(CreateUserDto dto)
        {
            return Ok(service.CreateUser(dto));
        }

        
        [HttpGet]
        public IActionResult Get()
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

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        
    }
}

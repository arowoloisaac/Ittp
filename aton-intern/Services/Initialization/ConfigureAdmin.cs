using aton_intern.DTOs;
using Aton_intern.Enums;
using Aton_intern.Models;
using Aton_intern.Services.UserService;

namespace Aton_intern.Services.Initialization
{
    public static class ConfigureAdmin
    {
        public static async Task ConfigureAdminAsync(this WebApplication app)
        {
            using var serviceScope = app.Services.CreateScope();

            var userConfig = serviceScope.ServiceProvider.GetService<IUserService>();

            if (userConfig == null)
            {
                throw new Exception("Unable to get service");
            }

            var getAdminCredientials = app.Configuration.GetSection("InitialAdminCredientials") == null 
                ? throw new Exception("Configuration does not exist")
                : app.Configuration.GetSection("InitialAdminCredientials");

            userConfig.CreateUser(new CreateUserDto
            {
                Username = getAdminCredientials["Username"],
                Password = getAdminCredientials["Password"],
                Gender = Gender.Unknown,
                IsAdmin = true,
                Name = getAdminCredientials["Name"],
                BirthDate = DateTime.UtcNow.AddYears(-20)
            }, createdBy: "system");
        }
    }
}

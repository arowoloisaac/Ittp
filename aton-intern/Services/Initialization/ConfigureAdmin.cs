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

            var getAdminCredientials = app.Configuration.GetSection("InitialAdminCredientials") == null 
                ? throw new Exception("Configuration does not exist")
                : app.Configuration.GetSection("InitialAdminCredientials");

            //var checkAdminExistence = userConfig.IsUserExists(getAdminCredientials["Username"]);

            userConfig.CreateUser(new User
            {
                Username = getAdminCredientials["Username"],
                CreatedBy = getAdminCredientials["CreatedBy"],
                Password = getAdminCredientials["Password"],
                Gender = Gender.Unknown,
                IsAdmin = true,
                Name = getAdminCredientials["Name"],
            });

            /*if (!checkAdminExistence)
            {
                 userConfig.CreateUser(new User
                {
                    Id = Guid.NewGuid(),
                    Username = getAdminCredientials["Username"],
                    CreatedBy = getAdminCredientials["CreatedBy"],
                    Password = getAdminCredientials["Password"],
                    Gender = Gender.Male,
                    IsAdmin = true,
                    Name = getAdminCredientials["Name"],
                    CreatedOn = DateTime.UtcNow,
                });
            }*/
        }
    }
}

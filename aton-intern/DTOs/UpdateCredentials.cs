using Aton_intern.Enums;

namespace aton_intern.DTOs
{
    public class UpdateCredentials
    {
        public string Username { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public Gender Gender {  get; set; }

        public string Password { get; set; } = string.Empty;
            
        public DateTime BirthDate { get; set; }
    }
}

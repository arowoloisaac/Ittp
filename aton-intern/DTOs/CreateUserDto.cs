using Aton_intern.Enums;
using System.ComponentModel.DataAnnotations;

namespace aton_intern.DTOs
{
    public class CreateUserDto
    {
        [RegularExpression(@"^([A-Za-zА-Яа-яЁё]+$)", ErrorMessage = "Allows only latin and cyrillic")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username can not be empty")]
        [RegularExpression(@"^([A-Za-z0-9]+$)", ErrorMessage = "Allows only latin and numbers")]
        public string Username { get; set; } = string.Empty;

        [MinLength(4, ErrorMessage = "Password can not be less than 4")]
        [RegularExpression(@"^([A-Za-z0-9]+$)", ErrorMessage = "Allows only latin and numbers")]
        public required string Password { get; set; }

        public Gender Gender { get; set; }

        public DateTime BirthDate { get; set; } = DateTime.UtcNow.AddYears(-10);

        public bool IsAdmin { get; set; } = false;
    }
}

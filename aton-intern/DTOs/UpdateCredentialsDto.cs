using Aton_intern.Enums;

namespace aton_intern.DTOs
{
    public class UpdateCredentialsDto
    {
        public string Name { get; set; } = string.Empty;

        public Gender? Gender {  get; set; }
            
        public DateTime? BirthDate { get; set; }
    }
}

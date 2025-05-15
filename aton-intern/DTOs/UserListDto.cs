using Aton_intern.Enums;

namespace aton_intern.DTOs
{
    public class UserListDto
    {
        public string UserName { get; set; }  = string.Empty;

        public string Name { get; set; } = string.Empty;

        public Gender Gender { get; set; }

        public DateTime Birthday { get; set; }

        public bool IsActive { get; set; }
    }
}

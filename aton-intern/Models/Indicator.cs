namespace Aton_intern.Models
{
    public class Indicator
    {
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? CreatedOn { get; set; }

        public string ModifiedBy { get; set; } = string.Empty;

        public DateTime? ModifiedOn { get; set; }

        public string RevokedBy { get; set; } = string.Empty;

        public DateTime? RevokedOn {  get; set; }

        public bool IsActive => RevokedOn == null;
    }
}

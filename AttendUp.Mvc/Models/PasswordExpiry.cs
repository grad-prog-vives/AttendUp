using System.ComponentModel.DataAnnotations;

namespace AttendUp.Mvc.Models
{
    public class PasswordExpiry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime LastPasswordChanged { get; set; } = DateTime.Now;

        [Required]
        public bool IsBlockedDueToExpiry { get; set; } = false;
    }
}

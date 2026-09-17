using System.ComponentModel.DataAnnotations.Schema;

namespace AttendUp.Mvc.Models
{
    [Table("Registration")]
    public class Registration
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int TrainingID { get; set; }
        public Training Training { get; set; }

        public int SubActivityID { get; set; }
        public SubActivity SubActivity { get; set; }

        public int LocationId { get; set; }
        public Location Location { get; set; }

        public DateTime RegisteredAt { get; set; }
        [Column("isValid")]
        public bool IsValid { get; set; }

        public string? IpAddress { get; set; } 
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace AttendUp.Mvc.Models
{
    [Table("Training")]
    public class Training
    {
        public ICollection<TrainingSubActivity>? TrainingSubActivities { get; set; }
        public string Title { get; set; }
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsActive { get; set; } = true;

        public string? DeactivationReason { get; set; }
    }
}

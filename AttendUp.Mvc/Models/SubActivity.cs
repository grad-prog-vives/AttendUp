using System.ComponentModel.DataAnnotations.Schema;

namespace AttendUp.Mvc.Models
{
    [Table("SubActivity")]
    public class SubActivity
    {
        public ICollection<TrainingSubActivity>? TrainingSubActivities { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
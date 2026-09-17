using System.ComponentModel.DataAnnotations.Schema;

namespace AttendUp.Mvc.Models
{
    [Table("TrainingSubActivity")]
    public class TrainingSubActivity
    {
        public int TrainingID { get; set; }
        public Training Training { get; set; }
        public int SubActivityID { get; set; }
        public SubActivity SubActivity { get; set; }
    }
}
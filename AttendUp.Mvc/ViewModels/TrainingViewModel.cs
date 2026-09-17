using AttendUp.Mvc.Models;
using System.ComponentModel.DataAnnotations;

namespace AttendUp.Mvc.ViewModels
{
    public class TrainingViewModel
    {
        [Required]
        public string Title { get; set; }

        public int ID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public List<int> SelectedSubActivityIDs { get; set; } = new();
        public List<SubActivity> AllSubActivities { get; set; } = new();
    }
}
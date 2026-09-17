using AttendUp.Mvc.Models;
using System.ComponentModel.DataAnnotations;

namespace AttendUp.Mvc.ViewModels
{
    public class SeasonPlanningViewModel
    {
        [Required(ErrorMessage = "Titel is verplicht")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Startdatum is verplicht")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Einddatum is verplicht")]
        public DateTime EndDate { get; set; }

        public List<DayOfWeek> SelectedDays { get; set; } = new();

        [Required(ErrorMessage = "Startuur is verplicht")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "Einduur is verplicht")]
        public TimeSpan EndTime { get; set; }

        public List<int> SelectedSubActivityIDs { get; set; } = new();
        public List<SubActivity> AllSubActivities { get; set; } = new();
    }
}

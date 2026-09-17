using AttendUp.Mvc.Models;

namespace AttendUp.Mvc.ViewModels
{
    public class ExportViewModel
    {
        public List<Training> Trainings { get; set; } = new();
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}

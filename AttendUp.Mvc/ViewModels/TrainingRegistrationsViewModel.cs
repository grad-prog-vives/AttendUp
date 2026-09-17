namespace AttendUp.Mvc.ViewModels
{
    public class TrainingRegistrationsViewModel
    {
        public int TrainingID { get; set; }
        public string TrainingTitle { get; set; }
        public DateTime TrainingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public List<string> ActiveSubActivities { get; set; } = new();
        public List<AdminRegistrationViewModel> Registrations { get; set; } = new();
        public int TotalRegistrations => Registrations.Count;
    }
}

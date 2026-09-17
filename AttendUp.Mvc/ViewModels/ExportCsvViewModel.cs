namespace AttendUp.Mvc.ViewModels
{
    public class ExportCsvViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TrainingTitle { get; set; }
        public DateTime TrainingDate { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string SubActivity { get; set; }
        public string Location { get; set; }
    }
}
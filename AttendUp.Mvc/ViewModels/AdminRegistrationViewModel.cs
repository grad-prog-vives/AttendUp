namespace AttendUp.Mvc.ViewModels
{
    public class AdminRegistrationViewModel
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SubActivityName { get; set; }
        public int SubActivityID { get; set; }
        public DateTime RegisteredAt { get; set; }
        public int TrainingID { get; set; }
        public bool IsDuplicate { get; set; }
    }
}

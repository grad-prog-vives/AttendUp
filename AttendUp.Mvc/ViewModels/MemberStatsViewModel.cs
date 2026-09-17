namespace AttendUp.Mvc.ViewModels
{
    public class MemberStatsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RegistrationCount { get; set; }
        public DateTime LastActive { get; internal set; }
    }
}

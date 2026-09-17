namespace AttendUp.Mvc.ViewModels
{
    public class DashboardViewModel
    {
        public int TodayAttendance { get; set; }
        public int TodayPercentChange { get; set; }

        public double AveragePerTraining { get; set; }
        public int AveragePercentChange { get; set; }

        public int ActiveTrainingsThisWeek { get; set; }

        public List<MemberStatsViewModel> TopMembers { get; set; }

        public bool HasSeedingData { get; set; }
    }
}

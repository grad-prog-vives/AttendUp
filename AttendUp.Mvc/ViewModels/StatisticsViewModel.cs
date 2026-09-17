namespace AttendUp.Mvc.ViewModels
{
    public class StatisticsViewModel
    {
        public int TodayAttendance { get; set; }
        public int TodayPercentChange { get; set; }

        public double AveragePerTraining { get; set; }
        public int AveragePercentChange { get; set; }

        public int ActiveTrainingsThisWeek { get; set; } = 100;

        public List<SubActivityStatisticsViewModel> SubActivities { get; set; }

        public List<MemberStatsViewModel> TopMembers { get; set; }

        public int TotalMembers { get; internal set; }

        public int ActiveMembersThisMonth { get; internal set; }
        
        public List<ActiveMembersPerMonthViewModel> ActiveMembersPerMonth { get; set; }
    }
}

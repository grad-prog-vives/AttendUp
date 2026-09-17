using AttendUp.Mvc.ViewModels;


namespace AttendUp.Mvc.Services.Interfaces
{
    public interface IStatisticsService
    {

        int GetAttendance(DateTime start, DateTime end, bool uniqueMembers = false);

        int GetTotalMembers();

        List<MemberStatsViewModel> GetAllMembers(int take);

        int GetActiveMembersThisMonth();

        List<int> GetTrainingCounts();

        List<ActiveMembersPerMonthViewModel> GetActiveMembersPerMonth(List<DateTime> months);

        List<DateTime> GetMonths(DateTime today);

        List<SubActivityStatisticsViewModel> GetSubActivitiesWithAttendance();

        List<ExportCsvViewModel> GetExportRows(List<int> trainingIds);
    }
}

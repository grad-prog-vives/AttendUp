using AttendUp.Mvc.Data;
using AttendUp.Mvc.Services.Interfaces;
using AttendUp.Mvc.ViewModels;

namespace AttendUp.Mvc.Services;

public class StatisticsService : IStatisticsService
{
    private readonly AttendUpContext _context;

    public StatisticsService(AttendUpContext context)
    {
        _context = context;
    }

    // Calculates the total number of registrations within the specified date range.
    public int GetAttendance(DateTime start, DateTime end, bool uniqueMembers = false)
    {
        var query =
            from r in _context.Registrations
            join t in _context.Trainings
                on r.TrainingID equals t.ID
            where t.Date >= start && t.Date < end
            select r;

        if (uniqueMembers)
        {
            return query
                .Select(r => new { r.FirstName, r.LastName })
                .Distinct()
                .Count();
        }

        return query.Count();
    }

    // Calculates the total number of unique members based on first and last name.
    public int GetTotalMembers()
    {
        return _context.Registrations
            .Select(r => new { r.FirstName, r.LastName })
            .Distinct()
            .Count();
    }

    // Retrieves member statistics, including registration count and last activity date. Can return all members or only the top members based on registration count.
    public List<MemberStatsViewModel> GetAllMembers(int take)
    {
        var query = _context.Registrations
            .GroupBy(r => new { r.FirstName, r.LastName })
            .Select(g => new MemberStatsViewModel
            {
                FirstName = g.Key.FirstName,
                LastName = g.Key.LastName,
                RegistrationCount = g.Count(),
                LastActive = g.Max(r => r.RegisteredAt)
            })
            .OrderByDescending(m => m.RegistrationCount);

        query = take > -1 ? (IOrderedQueryable<MemberStatsViewModel>)query.Take(take) : query;

        return query.ToList();
    }


    // Calculates the number of unique members who have registered during the current month.
    public int GetActiveMembersThisMonth() =>
            _context.Registrations
            .Where(r =>
                r.RegisteredAt.Month == DateTime.Now.Month &&
                r.RegisteredAt.Year == DateTime.Now.Year)
            .Select(r => new { r.FirstName, r.LastName })
            .Distinct()
            .Count();

    // Retrieves the registration count for each training session.
    public List<int> GetTrainingCounts() =>
        _context.Registrations
            .GroupBy(r => r.TrainingID)
            .Select(g => g.Count())
            .ToList();

    // Calculates the number of unique active members for each specified month and maps the results to a view model.
    public List<ActiveMembersPerMonthViewModel> GetActiveMembersPerMonth(List<DateTime> months) =>
        months
        .Select(month => new ActiveMembersPerMonthViewModel
        {
            Month = month.ToString("MMM yyyy"),
            ActiveMembers = _context.Registrations
                .Where(r =>
                    r.RegisteredAt.Month == month.Month &&
                    r.RegisteredAt.Year == month.Year)
                .Select(r => new
                {
                    r.FirstName,
                    r.LastName
                })
                .Distinct()
                .Count()
        })
        .ToList();

    // Generates a chronologically ordered list containing the current month and the previous seven months
    public List<DateTime> GetMonths(DateTime today) =>
        Enumerable.Range(0, 8)
        .Select(i => new DateTime(today.Year, today.Month, 1).AddMonths(-i))
        .OrderBy(d => d)
        .ToList();

    // Retrieves all sub-activities and calculates the total number of registrations/attendees for each sub-activity.
    public List<SubActivityStatisticsViewModel> GetSubActivitiesWithAttendance() =>
        _context.SubActivities
            .Select(sa => new SubActivityStatisticsViewModel
            {
                SubActivityId = sa.ID,
                Name = sa.Name,

                AttendanceCount = _context.Registrations
                    .Count(r =>
                        r.SubActivityID == sa.ID)
            })
            .ToList();

    public List<ExportCsvViewModel> GetExportRows(List<int> trainingIds)
    {
        return (from r in _context.Registrations
                where trainingIds.Contains(r.TrainingID)
                join t in _context.Trainings on r.TrainingID equals t.ID
                join s in _context.SubActivities on r.SubActivityID equals s.ID
                join l in _context.Locations on r.LocationId equals l.Id
                orderby t.Date, r.LastName
                select new ExportCsvViewModel
                {
                    FirstName = r.FirstName,
                    LastName = r.LastName,
                    TrainingTitle = t.Title,
                    TrainingDate = t.Date,
                    SubActivity = s.Name,
                    Location = l.Name,
                    RegisteredAt = r.RegisteredAt
                }).ToList();
    }
}
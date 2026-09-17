namespace AttendUp.Mvc.ViewModels
{
    public class AdminRegistrationIndexViewModel
    {
        public List<TrainingRegistrationsViewModel> TrainingGroups { get; set; } = new();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public DateTime? SelectedDate { get; set; }
    }
}

namespace AttendUp.Mvc.ViewModels
{
    public class NotFoundViewModel
    {
        public DateTime? NextTrainingDate { get; set; }
        public TimeSpan? NextTrainingStartTime { get; set; }
        public TimeSpan? NextTrainingEndTime { get; set; }
        public bool HasNextTraining => NextTrainingDate.HasValue;

        public bool TodayTrainingDeactivated { get; set; }
        public string? DeactivationReason { get; set; }
    }
}

using AttendUp.Mvc.Models;

namespace AttendUp.Mvc.ViewModels
{
    public class RegistrationViewModel
    {
        public int TrainingID { get; set; }
        public int LocationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SubActivityID { get; set; }
        public int TotalPersons { get; set; }
        public List<ExtraPersonInput> ExtraPersons { get; set; } = new();
        public List<SubActivity> SubActivities { get; set; } = new();
        public List<TrainingSubActivity> TrainingSubActivities { get; set; }
        public List<FamilyGroup> FamilyGroups { get; set; } = new();
    }

    public class ExtraPersonInput
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int SubActivityID { get; set; }
    }

    public class FamilyGroup
    {
        public int FamilyId { get; set; }
        public string FamilyName { get; set; } = "";
        public List<FamilyMemberOption> Members { get; set; } = new();
    }

    public class FamilyMemberOption
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
    }
}

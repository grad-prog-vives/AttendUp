using System.ComponentModel.DataAnnotations;

namespace AttendUp.Mvc.ViewModels
{
    public class FamilyEditViewModel
    {
        public int FamilyId { get; set; }

        [Required(ErrorMessage = "Familienaam is verplicht")]
        public string FamilyName { get; set; }

        public List<FamilyMemberEditViewModel> Members { get; set; } = new();
    }

    public class FamilyMemberEditViewModel
    {
        public int FamilyMemberId { get; set; }

        [Required(ErrorMessage = "Voornaam is verplicht")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Achternaam is verplicht")]
        public string LastName { get; set; }

        public bool IsDeleted { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace AttendUp.Mvc.ViewModels
{
    public class FamilyCreateViewModel
    {
        [Required(ErrorMessage = "Familienaam is verplicht")]
        public string FamilyName { get; set; }

        public List<FamilyMemberViewModel> Members { get; set; } = new List<FamilyMemberViewModel>();
    }

    public class FamilyMemberViewModel
    {
        [Required(ErrorMessage = "Voornaam is verplicht")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Achternaam is verplicht")]
        public string LastName { get; set; }
    }
}
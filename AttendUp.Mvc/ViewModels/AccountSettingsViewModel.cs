using System.ComponentModel.DataAnnotations;

namespace AttendUp.Mvc.ViewModels
{
    public class AccountSettingsViewModel
    {
        [Required(ErrorMessage = "Voornaam is verplicht")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Achternaam is verplicht")]
        public required string LastName { get; set; }
    }
}

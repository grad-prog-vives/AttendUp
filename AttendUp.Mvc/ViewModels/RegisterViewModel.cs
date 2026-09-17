using System.ComponentModel.DataAnnotations;

namespace AttendUp.Mvc.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Voornaam is verplicht")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Achternaam is verplicht")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is verplicht")]
        [EmailAddress(ErrorMessage = "Ongeldig emailadres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [MinLength(6, ErrorMessage = "Wachtwoord moet minstens 6 tekens bevatten")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bevestig je wachtwoord")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Wachtwoorden komen niet overeen")]
        public string ConfirmPassword { get; set; }
    }
}

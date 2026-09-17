using System.ComponentModel.DataAnnotations;


namespace AttendUp.Mvc.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is verplicht")]
        [EmailAddress(ErrorMessage = "Ongeldig emailadres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Onthoud mij")]
        public bool RememberMe { get; set; }
    }
}

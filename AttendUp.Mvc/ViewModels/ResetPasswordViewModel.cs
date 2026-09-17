using System.ComponentModel.DataAnnotations;

namespace AttendUp.Mvc.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [StringLength(100, ErrorMessage = "Het {0} moet minstens {2} tekens lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nieuw Wachtwoord")]
        public string Password { get; set; }

        // HIER HEB IK HET AANGEPAST:
        // Ik heb de oude ConfirmPassword vervangen door jouw nieuwe versie met de verplichte foutmelding.
        [Required(ErrorMessage = "Het veld 'Bevestig wachtwoord' is verplicht.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "De wachtwoorden komen niet overeen.")]
        public string ConfirmPassword { get; set; }
    }
}
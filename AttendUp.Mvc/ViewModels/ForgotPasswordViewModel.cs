using System.ComponentModel.DataAnnotations;

namespace AttendUp.Mvc.ViewModels
{
    public class ForgotPasswordViewModel
    {
        // Ik geef aan dat dit veld verplicht is en een geldig e-mailadres moet bevatten.
        [Required(ErrorMessage = "Vul a.u.b. een e-mailadres in.")]
        [EmailAddress(ErrorMessage = "Vul een geldig e-mailadres in.")]
        public string Email { get; set; }
    }
}

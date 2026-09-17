using AttendUp.Mvc.Models;
using System.ComponentModel.DataAnnotations;

namespace AttendUp.Mvc.ViewModels
{
	public class EditRegistrationViewModel
	{
		public int ID { get; set; }

		[Required(ErrorMessage = "Voornaam is verplicht.")]
		[Display(Name = "Voornaam")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Achternaam is verplicht.")]
		[Display(Name = "Achternaam")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Subactiviteit is verplicht.")]
		[Display(Name = "Subactiviteit")]
		public int SubActivityID { get; set; }

		[Required]
		[Display(Name = "Geregistreerd op")]
		public DateTime RegisteredAt { get; set; }

		public IEnumerable<SubActivity> AvailableSubActivities { get; set; } = new List<SubActivity>();
	}
}



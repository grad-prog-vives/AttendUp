using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendUp.Mvc.Models
{
    public class FamilyMember
    {
        [Key]
        public int FamilyMemberId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        // FK naar Family
        public int FamilyId { get; set; }

        [ForeignKey(nameof(FamilyId))]
        public Family Family { get; set; }
    }
}
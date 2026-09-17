using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AttendUp.Mvc.Models
{
    public class Family
    {
        [Key]
        public int FamilyId { get; set; }

        [Required]
        public string FamilyName { get; set; }

        // FK naar AspNetUsers
        [Required]
        public string OwnerPersonId { get; set; }

        [ForeignKey(nameof(OwnerPersonId))]
        public IdentityUser OwnerPerson { get; set; }

        public ICollection<FamilyMember> FamilyMembers { get; set; }
    }
}
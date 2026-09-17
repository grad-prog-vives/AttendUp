using System.ComponentModel.DataAnnotations.Schema;

namespace AttendUp.Mvc.Models
{
    [Table("Location")]
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

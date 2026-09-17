using Microsoft.AspNetCore.Identity;

namespace AttendUp.Mvc.ViewModels
{
    public class AdminUserViewModel
    {
        public IdentityUser User { get; set; } = null!;
        public string Role { get; set; } = "Member";
        public bool IsSuperAdmin => Role == "SuperAdmin";
    }
}

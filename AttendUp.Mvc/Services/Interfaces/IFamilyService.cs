using AttendUp.Mvc.Models;
using AttendUp.Mvc.ViewModels;

namespace AttendUp.Mvc.Services.Interfaces
{
    public interface IFamilyService
    {
        Family GetFamilyWithMembers(int familyId, string userId);
        void UpdateFamily(FamilyEditViewModel model, string userId);
        void DeleteFamily(int familyId, string userId);
    }
}
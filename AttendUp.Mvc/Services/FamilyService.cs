using AttendUp.Mvc.Data;
using AttendUp.Mvc.Models;
using AttendUp.Mvc.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AttendUp.Mvc.ViewModels;

namespace AttendUp.Mvc.Services
{
    public class FamilyService : IFamilyService
    {
        private readonly AttendUpContext _context;

        public FamilyService(AttendUpContext context)
        {
            _context = context;
        }

        public Family GetFamilyWithMembers(int familyId, string userId)
        {
            return _context.Families
                .Include(f => f.FamilyMembers)
                .FirstOrDefault(f => f.FamilyId == familyId && f.OwnerPersonId == userId);
        }

        public void UpdateFamily(FamilyEditViewModel model, string userId)
        {
            var family = _context.Families
                .Include(f => f.FamilyMembers)
                .FirstOrDefault(f => f.FamilyId == model.FamilyId && f.OwnerPersonId == userId);

            if (family == null)
                return;

            family.FamilyName = model.FamilyName;

            foreach (var memberVm in model.Members)
            {
                if (memberVm.FamilyMemberId != 0)
                {
                    var existing = family.FamilyMembers
                        .FirstOrDefault(m => m.FamilyMemberId == memberVm.FamilyMemberId);

                    if (existing == null)
                        continue;

                    if (memberVm.IsDeleted)
                        _context.FamilyMembers.Remove(existing);
                    else
                    {
                        existing.FirstName = memberVm.FirstName;
                        existing.LastName = memberVm.LastName;
                    }
                }
                else if (!memberVm.IsDeleted)
                {
                    family.FamilyMembers.Add(new FamilyMember
                    {
                        FirstName = memberVm.FirstName,
                        LastName = memberVm.LastName
                    });
                }
            }

            _context.SaveChanges();
        }

        public void DeleteFamily(int familyId, string userId)
        {
            var family = _context.Families
                .Include(f => f.FamilyMembers)
                .FirstOrDefault(f => f.FamilyId == familyId && f.OwnerPersonId == userId);

            if (family == null)
                return;

            _context.FamilyMembers.RemoveRange(family.FamilyMembers);
            _context.Families.Remove(family);
            _context.SaveChanges();
        }
    }
}
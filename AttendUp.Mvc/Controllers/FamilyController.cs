using AttendUp.Mvc.Data;
using AttendUp.Mvc.Models;
using AttendUp.Mvc.Services.Interfaces;
using AttendUp.Mvc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AttendUp.Mvc.Controllers
{
    [Authorize(Roles = "Member,Admin,SuperAdmin")]
    public class FamilyController : BaseController
    {
        private readonly IFamilyService _familyService;

        public FamilyController(AttendUpContext context, IFamilyService familyService)
            : base(context)
        {
            _familyService = familyService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var families = _context.Families
                .Where(f => f.OwnerPersonId == userId)
                .Include(f => f.FamilyMembers)
                .ToList();

            LoadSettings();

            return View(families);
        }

        public IActionResult Create()
        {
            LoadSettings();
            return View();
        }

        public IActionResult Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var family = _familyService.GetFamilyWithMembers(id, userId);

            if (family == null)
                return NotFound();

            var model = new FamilyEditViewModel
            {
                FamilyId = family.FamilyId,
                FamilyName = family.FamilyName,
                Members = family.FamilyMembers.Select(m => new FamilyMemberEditViewModel
                {
                    FamilyMemberId = m.FamilyMemberId,
                    FirstName = m.FirstName,
                    LastName = m.LastName
                }).ToList()
            };

            LoadSettings();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, FamilyEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadSettings();
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.FamilyId = id;
            _familyService.UpdateFamily(model, userId);

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _familyService.DeleteFamily(id, userId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FamilyCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadSettings();
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var family = new Family
            {
                FamilyName = model.FamilyName,
                OwnerPersonId = userId,
                FamilyMembers = model.Members.Select(m => new FamilyMember
                {
                    FirstName = m.FirstName,
                    LastName = m.LastName
                }).ToList()
            };

            _context.Families.Add(family);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
using AttendUp.Mvc.Data;
using AttendUp.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendUp.Mvc.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class SubActivityController : Controller
    {
        private readonly AttendUpContext _context;

        public SubActivityController(AttendUpContext context)
        {
            _context = context;
        }

        [HttpGet("Admin/SubActivity/Create")]
        public IActionResult SubActivityCreate()
        {
            return View("~/Views/Admin/SubActivityCreate.cshtml", new SubActivity());
        }

        [HttpPost("Admin/SubActivity/Create")]
        [ValidateAntiForgeryToken]
        public IActionResult SubActivityCreate(SubActivity model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/SubActivityCreate.cshtml", model);

            var exists = _context.SubActivities
                .Any(s => s.Name.ToLower() == model.Name.ToLower());

            if (exists)
            {
                ModelState.AddModelError("Name", "Deze subactiviteit bestaat al.");
                return View("~/Views/Admin/SubActivityCreate.cshtml", model);
            }

            model.IsActive = true;

            _context.SubActivities.Add(model);
            _context.SaveChanges();

            return RedirectToAction("SubActivity","Admin");
        }

        [HttpGet("Admin/SubActivity/Edit/{id}")]
        public IActionResult SubActivityEdit(int id)
        {
            var subActivity = _context.SubActivities.Find(id);
            if (subActivity == null) return NotFound();
            return View("~/Views/Admin/SubActivityEdit.cshtml", subActivity);
        }

        [HttpPost("Admin/SubActivity/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult SubActivityEdit(SubActivity model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/SubActivityEdit.cshtml", model);

            var normalizedName = model.Name.Trim().ToLower();

            var exists = _context.SubActivities
                .Any(s => s.ID != model.ID &&
                          s.Name.Trim().ToLower() == normalizedName);

            if (exists)
            {
                ModelState.AddModelError("Name", "Deze subactiviteit bestaat al.");
                return View("~/Views/Admin/SubActivityEdit.cshtml", model);
            }

            var subActivity = _context.SubActivities.Find(model.ID);

            if (subActivity == null)
                return NotFound();

            subActivity.Name = model.Name;

            _context.SaveChanges();

            return RedirectToAction("SubActivity", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubActivityToggle(int id)
        {
            var subActivity = _context.SubActivities.Find(id);
            if (subActivity == null) return NotFound();

            subActivity.IsActive = !subActivity.IsActive;
            _context.SaveChanges();

            return RedirectToAction("SubActivity", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubActivityDelete(int id)
        {
            var subActivity = _context.SubActivities.Find(id);
            if (subActivity == null) return NotFound();

            var hasRegistrations = _context.Registrations.Any(r => r.SubActivityID == id);
            if (hasRegistrations)
            {
                TempData["ErrorMessage"] = "Kan niet verwijderen: deze sub-activiteit wordt nog gebruikt in registraties.";
                return RedirectToAction("SubActivity", "Admin");
            }

            _context.SubActivities.Remove(subActivity);
            _context.SaveChanges();

            return RedirectToAction("SubActivity", "Admin");
        }
    }
}

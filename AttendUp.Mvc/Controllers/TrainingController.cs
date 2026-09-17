using AttendUp.Mvc.Data;
using AttendUp.Mvc.Models;
using AttendUp.Mvc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AttendUp.Mvc.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class TrainingController : BaseController
    {
        private readonly AttendUpContext _context;

        public TrainingController(AttendUpContext context) : base(context)
        {
            _context = context;
        }

        [HttpGet("Admin/Training/Create")]
        public IActionResult Create()
        {
            var vm = new TrainingViewModel
            {
                AllSubActivities = _context.SubActivities.Where(s => s.IsActive).ToList()
            };
            return View("~/Views/Admin/TrainingCreate.cshtml", vm);
        }

        [HttpPost("Admin/Training/Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TrainingViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.AllSubActivities = _context.SubActivities.Where(s => s.IsActive).ToList();
                return View("~/Views/Admin/TrainingCreate.cshtml", vm);
            }

            var alreadyExists = _context.Trainings.Any(t => t.Date.Year == vm.Date.Year && t.Date.Month == vm.Date.Month && t.Date.Day == vm.Date.Day);

            if (alreadyExists)
            {
                ModelState.AddModelError("", "Er bestaat al een training op deze dag.");
                vm.AllSubActivities = _context.SubActivities.Where(s => s.IsActive).ToList();
                return View("~/Views/Admin/TrainingCreate.cshtml", vm);
            }

            var training = new Training
            {
                Title = vm.Title,
                Date = vm.Date,
                StartTime = vm.StartTime,
                EndTime = vm.EndTime,
                IsActive = true
            };

            if (vm.SelectedSubActivityIDs == null || vm.SelectedSubActivityIDs.Count == 0)
            {
                ModelState.AddModelError("", "Selecteer minstens één subactiviteit.");
                vm.AllSubActivities = _context.SubActivities.Where(s => s.IsActive).ToList();
                return View("~/Views/Admin/TrainingCreate.cshtml", vm);
            }

            _context.Trainings.Add(training);
            _context.SaveChanges();

            foreach (var subId in vm.SelectedSubActivityIDs)
            {
                _context.TrainingSubActivities.Add(new TrainingSubActivity
                {
                    TrainingID = training.ID,
                    SubActivityID = subId
                });
            }

            LogAudit("Aangemaakt", $"(ID: {training.ID}) Training '{training.Title}' aangemaakt");

            _context.SaveChanges();

            return RedirectToAction("Training", "Admin");
        }

        [HttpGet("Admin/TrainingEdit/{id}")]
        public IActionResult TrainingEdit(int id)
        {
            var training = _context.Trainings.Find(id);
            if (training == null) return NotFound();

            var selectedIds = _context.TrainingSubActivities.Where(ts => ts.TrainingID == id).Select(ts => ts.SubActivityID).ToList();

            var vm = new TrainingViewModel
            {
                ID = training.ID,
                Title = training.Title,
                Date = training.Date,
                StartTime = training.StartTime,
                EndTime = training.EndTime,
                SelectedSubActivityIDs = selectedIds,
                AllSubActivities = _context.SubActivities.Where(s => s.IsActive).ToList()
            };

            return View("~/Views/Admin/TrainingCreate.cshtml", vm);
        }

        [HttpPost("Admin/TrainingEdit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult TrainingEdit(int id, TrainingViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.AllSubActivities = _context.SubActivities.Where(s => s.IsActive).ToList();
                return View("~/Views/Admin/TrainingCreate.cshtml", vm);
            }

            var training = _context.Trainings.Find(id);
            if (training == null) return NotFound();

            var alreadyExists = _context.Trainings.Any(t => t.ID != id && t.Date.Year == vm.Date.Year && t.Date.Month == vm.Date.Month && t.Date.Day == vm.Date.Day);

            if (alreadyExists)
            {
                ModelState.AddModelError("", "Er bestaat al een training op deze dag.");
                vm.AllSubActivities = _context.SubActivities.Where(s => s.IsActive).ToList();
                return View("~/Views/Admin/TrainingCreate.cshtml", vm);
            }

            training.Title = vm.Title;
            training.Date = vm.Date;
            training.StartTime = vm.StartTime;
            training.EndTime = vm.EndTime;

            var existing = _context.TrainingSubActivities.Where(ts => ts.TrainingID == id).ToList();
            _context.TrainingSubActivities.RemoveRange(existing);

            if (vm.SelectedSubActivityIDs == null || vm.SelectedSubActivityIDs.Count == 0)
            {
                ModelState.AddModelError("", "Selecteer minstens 1 subactiviteit.");
                vm.AllSubActivities = _context.SubActivities.Where(s => s.IsActive).ToList();
                return View("~/Views/Admin/TrainingCreate.cshtml", vm);
            }

            foreach (var subId in vm.SelectedSubActivityIDs)
            {
                _context.TrainingSubActivities.Add(new TrainingSubActivity
                {
                    TrainingID = id,
                    SubActivityID = subId
                });
            }

            LogAudit("Aangepast", $"(ID: {id}) Training '{training.Title}' gewijzigd.");

            _context.SaveChanges();

            return RedirectToAction("Training", "Admin");
        }

        [HttpPost("Admin/TrainingToggle/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult TrainingToggle(int id)
        {
            var training = _context.Trainings.Find(id);
            if (training == null) return NotFound();

            if (training.IsActive)
            {
                return View("~/Views/Admin/TrainingDeactivate.cshtml", training);
            }

            training.DeactivationReason = null;
            training.IsActive = !training.IsActive;

            LogAudit("Aangepast", $"(ID: {id}) Training '{training.Title}' is op ACTIEF gezet.");

            _context.SaveChanges();

            return RedirectToAction("Training", "Admin");
        }

        [HttpPost("Admin/TrainingDeactivate/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult TrainingDeactivate(int id, string reason)
        {
            var training = _context.Trainings.Find(id);
            if (training == null) return NotFound();

            training.IsActive = !training.IsActive;
            training.DeactivationReason = reason;

            LogAudit("Aangepast", $"(ID: {id}) Training '{training.Title}' is GEDEACTIVEERD. Reden: {reason}");

            _context.SaveChanges();

            return RedirectToAction("Training", "Admin");
        }

        [HttpGet("Admin/Training/SeasonPlanning")]
        public IActionResult SeasonPlanning()
        {
            var vm = new SeasonPlanningViewModel
            {
                StartDate = new DateTime(DateTime.Today.Year, 9, 1),
                EndDate = new DateTime(DateTime.Today.Year + 1, 6, 30),
                AllSubActivities = _context.SubActivities.Where(s => s.IsActive).ToList()
            };
            return View("~/Views/Admin/SeasonPlanning.cshtml", vm);
        }

        [HttpPost("Admin/Training/SeasonPlanning")]
        [ValidateAntiForgeryToken]
        public IActionResult SeasonPlanning(SeasonPlanningViewModel vm)
        {
            if (vm.SelectedDays == null || vm.SelectedDays.Count == 0)
                ModelState.AddModelError("SelectedDays", "Selecteer minstens één dag.");

            if (vm.SelectedSubActivityIDs == null || vm.SelectedSubActivityIDs.Count == 0)
                ModelState.AddModelError("SelectedSubActivityIDs", "Selecteer minstens één subactiviteit.");

            if (vm.EndDate <= vm.StartDate)
                ModelState.AddModelError(string.Empty, "Einddatum moet na de startdatum liggen.");

            if (!ModelState.IsValid)
            {
                vm.AllSubActivities = _context.SubActivities.Where(s => s.IsActive).ToList();
                return View("~/Views/Admin/SeasonPlanning.cshtml", vm);
            }

            var existingDates = _context.Trainings
                .Select(t => t.Date.Date)
                .ToHashSet();

            var created = 0;
            var skipped = 0;
            var current = vm.StartDate.Date;

            while (current <= vm.EndDate.Date)
            {
                if (vm.SelectedDays.Contains(current.DayOfWeek))
                {
                    if (existingDates.Contains(current))
                    {
                        skipped++;
                    }
                    else
                    {
                        var training = new Training
                        {
                            Title = vm.Title,
                            Date = current,
                            StartTime = vm.StartTime,
                            EndTime = vm.EndTime,
                            IsActive = true
                        };
                        _context.Trainings.Add(training);
                        _context.SaveChanges();

                        foreach (var subId in vm.SelectedSubActivityIDs)
                        {
                            _context.TrainingSubActivities.Add(new TrainingSubActivity
                            {
                                TrainingID = training.ID,
                                SubActivityID = subId
                            });
                        }
                        _context.SaveChanges();
                        created++;
                    }
                }
                current = current.AddDays(1);
            }

            TempData["SeasonResult"] = $"{created} trainingen aangemaakt, {skipped} overgeslagen (dag bestond al).";
            return RedirectToAction("Training", "Admin");
        }

        [HttpPost("Admin/Delete/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var training = _context.Trainings.Find(id);
            if (training == null) return NotFound();

            var registrations = _context.Registrations.Where(r => r.TrainingID == id).ToList();
            _context.Registrations.RemoveRange(registrations);

            LogAudit("Verwijderd", $"(ID: {id}) Training '{training.Title}' permanent verwijderd inclusief {registrations.Count} bijbehorende registraties.");
            
            _context.SaveChanges();
            
            _context.Trainings.Remove(training);
            
            _context.SaveChanges();

            return RedirectToAction("Training", "Admin");
        }
    }
}
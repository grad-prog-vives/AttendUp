using AttendUp.Mvc.Data;
using AttendUp.Mvc.Models;
using AttendUp.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AttendUp.Mvc.Controllers
{
    public class RegistrationController : BaseController
    {
        public RegistrationController(AttendUpContext context)
            : base(context)
        {
        }

        public IActionResult Index(int? locationId)
        {
            LoadSettings();

            if (locationId == null || !_context.Locations.Any(l => l.Id == locationId && l.IsActive))
            {
                var today2 = DateTime.Today;
                var nextTraining2 = _context.Trainings
                    .Where(t => t.Date.Date > today2 && t.IsActive)
                    .OrderBy(t => t.Date)
                    .FirstOrDefault();

                var scanAgainVm = new NotFoundViewModel
                {
                    NextTrainingDate = nextTraining2?.Date,
                    NextTrainingStartTime = nextTraining2?.StartTime,
                    NextTrainingEndTime = nextTraining2?.EndTime,
                };
                return View("ScanAgain", scanAgainVm);
            }

            var today = DateTime.Today;
            var now = DateTime.Now;
            var training = _context.Trainings.Include(t => t.TrainingSubActivities).ThenInclude(tsa => tsa.SubActivity).FirstOrDefault(t => t.Date.Date == today && t.IsActive);

            if (training == null)
            {
                return ShowNotFound(today);
            }

            var settings = _context.Settings.FirstOrDefault();
            int windowMinutes = settings?.RegistrationWindowMinutes ?? 30;

            var trainingDate = training.Date.Date;
            var start = trainingDate + training.StartTime;
            var end = trainingDate + training.EndTime;

            // Het doorlopende venster: van X min voor de start tot X min na het einde
            var windowOpen = start.AddMinutes(-windowMinutes);
            var windowClosed = end.AddMinutes(windowMinutes);

            // Te vroeg
            if (now < windowOpen)
            {
                ViewBag.OpeningTime = windowOpen.ToString("HH:mm");
                return View("TooEarly");
            }

            // Te laat
            if (now > windowClosed)
            {
                return View("TooLate");
            }

            // Binnen de tijd! Toon het formulier
            var vm = new RegistrationViewModel
            {
                TrainingID = training.ID,
                LocationId = locationId.Value,
                SubActivities = training.TrainingSubActivities.Select(tsa => tsa.SubActivity).Where(s => s.IsActive).ToList()
            };

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var parts = User.Identity?.Name?.Split('.') ?? [];
                vm.FirstName = parts.Length > 0 ? parts[0] : string.Empty;
                vm.LastName = parts.Length > 1 ? string.Join(".", parts[1..]) : string.Empty;

                vm.FamilyGroups = _context.Families
                    .Include(f => f.FamilyMembers)
                    .Where(f => f.OwnerPersonId == userId)
                    .Select(f => new FamilyGroup
                    {
                        FamilyId = f.FamilyId,
                        FamilyName = f.FamilyName,
                        Members = f.FamilyMembers.Select(m => new FamilyMemberOption
                        {
                            MemberId = m.FamilyMemberId,
                            FirstName = m.FirstName,
                            LastName = m.LastName
                        }).ToList()
                    })
                    .ToList();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RegistrationViewModel vm)
        {
            var training = _context.Trainings.FirstOrDefault(t => t.ID == vm.TrainingID);


            if (training == null || !training.IsActive)
            {
                LoadSettings();
                return View("NotFound");
            }

            var now = DateTime.Now;
            var trainingDate = training.Date.Date;
            var start = trainingDate + training.StartTime;
            var end = trainingDate + training.EndTime;

            var settings = _context.Settings.FirstOrDefault();
            int windowMinutes = settings?.RegistrationWindowMinutes ?? 30;

            var windowOpen = start.AddMinutes(-windowMinutes);
            var windowClosed = end.AddMinutes(windowMinutes);

            if (now < windowOpen || now > windowClosed)
            {
                return RedirectToAction("Index");
            }
            // IP-logic
            string? ipAddress = null;
            if (settings != null && settings.StoreIpAddresses) // 
            {
                ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                // Check , reverse proxies (IIS, Nginx of Cloudflare)
                if (HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
                {
                    ipAddress = HttpContext.Request.Headers["X-Forwarded-For"].ToString();
                }
            }
            _context.Registrations.Add(new Registration
            {
                TrainingID = vm.TrainingID,
                LocationId = vm.LocationId,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                SubActivityID = vm.SubActivityID,
                IsValid = true,
                RegisteredAt = now,
                IpAddress = ipAddress 
            });

            if (vm.ExtraPersons != null)
            {
                foreach (var extra in vm.ExtraPersons)
                {
                    if (!string.IsNullOrWhiteSpace(extra.FirstName) && !string.IsNullOrWhiteSpace(extra.LastName))
                    {
                        _context.Registrations.Add(new Registration
                        {
                            TrainingID = vm.TrainingID,
                            LocationId = vm.LocationId,
                            FirstName = extra.FirstName,
                            LastName = extra.LastName,
                            SubActivityID = extra.SubActivityID,
                            IsValid = true,
                            RegisteredAt = now,
                            IpAddress = ipAddress
                        });
                    }
                }
            }

            _context.SaveChanges();
            LoadSettings();
            return RedirectToAction("Success");
        }

        private IActionResult ShowNotFound(DateTime today)
        {
            LoadSettings();
            var nextTraining = _context.Trainings
                .Where(t => t.Date.Date > today && t.IsActive)
                .OrderBy(t => t.Date)
                .FirstOrDefault();

            var todayDeactivated = _context.Trainings.FirstOrDefault(t =>
                 t.Date.Date == today && !t.IsActive);

            var notFoundVm = new NotFoundViewModel
            {
                NextTrainingDate = nextTraining?.Date,
                NextTrainingStartTime = nextTraining?.StartTime,
                NextTrainingEndTime = nextTraining?.EndTime,
                TodayTrainingDeactivated = todayDeactivated != null,
                DeactivationReason = todayDeactivated?.DeactivationReason,
            };

            return View("NotFound", notFoundVm);
        }

        public IActionResult Success()
        {
            LoadSettings();
            return View();
        }
    }
}
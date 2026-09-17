using AttendUp.Mvc.Data;
using AttendUp.Mvc.Models;
using AttendUp.Mvc.Services.Interfaces;
using AttendUp.Mvc.ViewModels;
using Microsoft.AspNetCore.Authorization; // I add this to fix the authorization issue 
using Microsoft.AspNetCore.Identity; // I add this to be able to work with Identity users.
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // I add this to be able to use .ToListAsync().
using System.Data;
using System.Text;

namespace AttendUp.Mvc.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")] // I add this to fix the authorization issue 
    public class AdminController : BaseController
    {
        private readonly IStatisticsService _statisticsService;
        private readonly IEmailService _emailService;
        // I add the UserManager so I can manage all user accounts within this controller.
        private readonly UserManager<IdentityUser> _userManager;

        // I request the UserManager via the constructor so ASP.NET injects it automatically.
        // We are now calling base() empty, or we pass exactly what the BaseController expects
        public AdminController(
            AttendUpContext context,
            IStatisticsService statisticsService,
            UserManager<IdentityUser> userManager,
            IEmailService emailService)
            : base(context) // If base(context) remains red, change it to base()
        {

            _statisticsService = statisticsService;
            _emailService = emailService;
            _userManager = userManager; // I store the manager here for use in the actions.
        }

        public IActionResult Index()
        {
            var today = DateTime.Today;
            int diffToMonday = ((int)today.DayOfWeek + 6) % 7;

            var todayAttendance = _statisticsService.GetAttendance(today, today.AddDays(1), true);

            var startOfWeek = today.AddDays(-diffToMonday);
            var endOfWeek = startOfWeek.AddDays(7);

            var startOfLastWeek = startOfWeek.AddDays(-7);
            var endOfLastWeek = startOfWeek;

            var thisWeekAttendance = _statisticsService.GetAttendance(startOfWeek, endOfWeek);

            var lastWeekAttendance = _statisticsService.GetAttendance(startOfLastWeek, endOfLastWeek);

            int todayPct = lastWeekAttendance == 0 ? 0 : (thisWeekAttendance - lastWeekAttendance) * 100 / lastWeekAttendance;

            var activeThisWeek = _context.Trainings.Count(t => t.Date >= startOfWeek && t.Date < endOfWeek);
            double thisWeekAvg = activeThisWeek == 0 ? 0 : (double)thisWeekAttendance / activeThisWeek;

            var lastWeekTrainings = _context.Trainings.Count(t => t.Date >= startOfLastWeek && t.Date < endOfLastWeek);
            double lastWeekAvg = lastWeekTrainings == 0 ? 0 : (double)lastWeekAttendance / lastWeekTrainings;

            double averagePercentChange = lastWeekAvg == 0 ? 0 : (thisWeekAvg - lastWeekAvg) * 100 / lastWeekAvg;

            var topMembers = _statisticsService.GetAllMembers(10);

            var vm = new DashboardViewModel
            {
                TodayAttendance = todayAttendance,
                TodayPercentChange = todayPct,
                AveragePerTraining = Math.Round(thisWeekAvg, 0),
                AveragePercentChange = (int)Math.Round(averagePercentChange, 2),
                ActiveTrainingsThisWeek = activeThisWeek,
                TopMembers = topMembers,
                HasSeedingData = _context.Trainings.Any()
            };
            LoadSettings();
            return View(vm);
        }

        [HttpGet("Admin/Members")]
        public IActionResult ViewAllMembers()
        {
            var allMembers = _statisticsService.GetAllMembers(-1);

            var vm = new DashboardViewModel
            {
                TopMembers = allMembers
            };
            LoadSettings();

            return View("~/Views/Admin/DashboardAllMembers.cshtml", vm);
        }

        public IActionResult Training()
        {
            var today = DateTime.Today;
            var trainings = _context.Trainings.OrderByDescending(t => t.Date.Date == today).ThenByDescending(t => t.Date).ToList();
            LoadSettings();
            return View(trainings);
        }


        public IActionResult SubActivity()
        {
            var subActivities = _context.SubActivities.OrderByDescending(s => s.IsActive).ToList();
            LoadSettings();
            return View(subActivities);
        }

        public IActionResult Location()
        {
            var locations = _context.Locations.OrderByDescending(l => l.IsActive).ToList();
            LoadSettings();
            return View(locations);
        }
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Settings()
        {
            var settings = _context.Settings.FirstOrDefault() ?? new Setting();
            LoadSettings();
            return View(settings);
        }

        public IActionResult Registrations(int page = 1, bool? showOnlyDuplicates = null)
        {
            bool filterOnDuplicates = false;

            if (showOnlyDuplicates.HasValue)
            {
                CookieOptions options = new CookieOptions { Expires = DateTime.Now.AddDays(30) };
                Response.Cookies.Append("ShowOnlyDuplicatesFilter", showOnlyDuplicates.Value.ToString(), options);
                filterOnDuplicates = showOnlyDuplicates.Value;
            }
            else
            {
                if (Request.Cookies.TryGetValue("ShowOnlyDuplicatesFilter", out string cookieValue))
                {
                    bool.TryParse(cookieValue, out filterOnDuplicates);
                }
            }

            ViewBag.ShowingOnlyDuplicates = filterOnDuplicates;

            var trainings = _context.Trainings
                .Where(t => t.Date.Date <= DateTime.Today)
                .OrderByDescending(t => t.Date)
                .ToList();

            var trainingIds = trainings.Select(t => t.ID).ToList();

            var allRegistrations = _context.Registrations
                .Where(r => trainingIds.Contains(r.TrainingID))
                .Join(_context.SubActivities,
                    r => r.SubActivityID,
                    s => s.ID,
                    (r, s) => new AdminRegistrationViewModel
                    {
                        ID = r.ID,
                        FirstName = r.FirstName,
                        LastName = r.LastName,
                        SubActivityName = s.Name,
                        SubActivityID = r.SubActivityID,
                        RegisteredAt = r.RegisteredAt,
                        TrainingID = r.TrainingID
                    })
                .ToList();

            var trainingSubActivities = _context.TrainingSubActivities
                .Where(tsa => trainingIds.Contains(tsa.TrainingID))
                .Join(_context.SubActivities,
                    tsa => tsa.SubActivityID,
                    s => s.ID,
                    (tsa, s) => new { tsa.TrainingID, s.Name, s.IsActive })
                .ToList();

            var groups = trainings.Select(t => new TrainingRegistrationsViewModel
            {
                TrainingID = t.ID,
                TrainingTitle = t.Title,
                TrainingDate = t.Date,
                StartTime = t.StartTime,
                EndTime = t.EndTime,
                ActiveSubActivities = trainingSubActivities
                    .Where(tsa => tsa.TrainingID == t.ID && tsa.IsActive)
                    .Select(tsa => tsa.Name)
                    .ToList(),
                Registrations = allRegistrations
                    .Where(r => r.TrainingID == t.ID)
                    .OrderBy(r => r.RegisteredAt)
                    .ToList()
            }).ToList();

            foreach (var group in groups)
            {
                var duplicateNames = group.Registrations
                    .GroupBy(r => new { r.FirstName, r.LastName })
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                foreach (var reg in group.Registrations)
                {
                    if (duplicateNames.Any(d => d.FirstName == reg.FirstName && d.LastName == reg.LastName))
                    {
                        reg.IsDuplicate = true;
                    }
                }
            }

            if (filterOnDuplicates)
            {
                groups = groups.Where(g => g.Registrations.Any(r => r.IsDuplicate)).ToList();
            }

            int pageSize = 8;
            int totalItems = groups.Count;
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            if (totalPages == 0) totalPages = 1;

            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var pagedGroups = groups
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var vm = new AdminRegistrationIndexViewModel
            {
                TrainingGroups = pagedGroups,
                CurrentPage = page,
                TotalPages = totalPages
            };

            LoadSettings();
            return View(vm);
        }

        [HttpGet("Admin/Registrations/Edit/{id}")]
        public IActionResult EditRegistration(int id)
        {
            var registration = _context.Registrations.Find(id);
            if (registration == null) return NotFound();

            var trainingSubActivityIds = _context.TrainingSubActivities
                .Where(tsa => tsa.TrainingID == registration.TrainingID)
                .Select(tsa => tsa.SubActivityID)
                .ToList();

            var vm = new EditRegistrationViewModel
            {
                ID = registration.ID,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                SubActivityID = registration.SubActivityID,
                RegisteredAt = registration.RegisteredAt,
                AvailableSubActivities = _context.SubActivities
                    .Where(s => trainingSubActivityIds.Contains(s.ID) && s.IsActive)
                    .OrderBy(s => s.Name)
                    .ToList()
            };

            LoadSettings();
            return View("~/Views/Admin/RegistrationEdit.cshtml", vm);
        }

        [HttpPost("Admin/Registrations/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult EditRegistration(EditRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var registration = _context.Registrations.Find(model.ID);
                var trainingSubActivityIds = _context.TrainingSubActivities
                    .Where(tsa => tsa.TrainingID == registration!.TrainingID)
                    .Select(tsa => tsa.SubActivityID)
                    .ToList();

                model.AvailableSubActivities = _context.SubActivities
                    .Where(s => trainingSubActivityIds.Contains(s.ID) && s.IsActive)
                    .OrderBy(s => s.Name)
                    .ToList();

                LoadSettings();
                return View("~/Views/Admin/RegistrationEdit.cshtml", model);
            }

            var reg = _context.Registrations.Find(model.ID);
            if (reg == null) return NotFound();

            reg.FirstName = model.FirstName.Trim();
            reg.LastName = model.LastName.Trim();
            reg.SubActivityID = model.SubActivityID;
            reg.RegisteredAt = model.RegisteredAt;

            _context.SaveChanges();

            TempData["Success"] = "Registratie succesvol bijgewerkt.";
            return RedirectToAction("Registrations");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            var reg = await _context.Registrations.FindAsync(id);
            if (reg != null)
            {
                _context.Registrations.Remove(reg);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Dubbele registratie succesvol verwijderd.";
            }
            return RedirectToAction(nameof(Registrations));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")] // Access only SuperAdmin
        public async Task<IActionResult> Settings(Setting model, IFormFile? logoFile, IFormFile? eventBannerFile, bool resetLogo = false, bool resetEventBanner = false)
        {
            ModelState.Remove("LogoPath");
            ModelState.Remove("DefaultButtonColor");
            ModelState.Remove("DefaultBackgroundColor");
            ModelState.Remove("EventBannerPath");

            if (!ModelState.IsValid)
                return View(model);

            var existing = _context.Settings.FirstOrDefault();

            if (existing == null)
            {
                if (logoFile != null && logoFile.Length > 0)
                {
                    var fileName = "current-logo" + Path.GetExtension(logoFile.FileName);
                    var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                    using var stream = new FileStream(savePath, FileMode.Create);
                    await logoFile.CopyToAsync(stream);
                    model.LogoPath = "/images/" + fileName;
                }
                if (eventBannerFile != null && eventBannerFile.Length > 0)
                {
                    var fileName = "event-banner" + Path.GetExtension(eventBannerFile.FileName);
                    var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                    using var stream = new FileStream(savePath, FileMode.Create);
                    await eventBannerFile.CopyToAsync(stream);
                    model.EventBannerPath = "/images/" + fileName;
                }
                _context.Settings.Add(model);
            }
            else
            {
                existing.ButtonColor = model.ButtonColor;
                existing.BackgroundColor = model.BackgroundColor;
                existing.DefaultButtonColor = model.DefaultButtonColor;
                existing.DefaultBackgroundColor = model.DefaultBackgroundColor;
                existing.RegistrationWindowMinutes = model.RegistrationWindowMinutes;

                existing.BoardSessionTimeoutMinutes = model.BoardSessionTimeoutMinutes;
                existing.StoreIpAddresses = model.StoreIpAddresses; // Store IP addresses setting

                if (logoFile != null && logoFile.Length > 0)
                {
                    var fileName = "current-logo" + Path.GetExtension(logoFile.FileName);
                    var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
                    using var stream = new FileStream(savePath, FileMode.Create);
                    await logoFile.CopyToAsync(stream);
                    existing.LogoPath = "/images/" + fileName;
                }

                if (resetLogo)
                    existing.LogoPath = null;

                if (resetEventBanner)
                    existing.EventBannerPath = null;
            }
            var cache = HttpContext.RequestServices.GetRequiredService<Microsoft.Extensions.Caching.Memory.IMemoryCache>();
            cache.Remove("BoardSessionTimeout"); 

            _context.SaveChanges();
            TempData["Success"] = "Instellingen opgeslagen!";
            return RedirectToAction("Settings");
        }


        public IActionResult Statistics()
        {
            var trainingCounts = _statisticsService.GetTrainingCounts();

            double averagePerTraining = trainingCounts.Count != 0 ? trainingCounts.Average() : 0;

            LoadSettings();

            return View("~/Views/Admin/Statistics.cshtml", new StatisticsViewModel
            {
                TopMembers = _statisticsService.GetAllMembers(10), // take 10 members
                TotalMembers = _statisticsService.GetTotalMembers(),
                ActiveMembersThisMonth = _statisticsService.GetActiveMembersThisMonth(),
                AveragePerTraining = Math.Round(averagePerTraining, 0),
                ActiveMembersPerMonth = _statisticsService.GetActiveMembersPerMonth(_statisticsService.GetMonths(DateTime.Today)),
                SubActivities = _statisticsService.GetSubActivitiesWithAttendance(),
            });
        }

        public IActionResult Export(DateTime? start, DateTime? end)
        {
            if (!_context.Trainings.Any())
            {
                return View(new ExportViewModel
                {
                    Trainings = new List<Training>(),
                    Start = start,
                    End = end
                });
            }

            var trainings = _context.Trainings.AsQueryable();
            start ??= trainings.Min(t => t.Date);
            end ??= trainings.Max(t => t.Date);

            LoadSettings();
            return View(new ExportViewModel
            {
                Trainings = trainings.Where(t => t.Date >= start.Value)
                .Where(t => t.Date <= end.Value)
                .OrderByDescending(t => t.Date).ToList(),
                Start = start,
                End = end
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExportCsv(List<int> trainingIds)
        {
            var rows = _statisticsService.GetExportRows(trainingIds);

            var lines = new List<string> { "Voornaam;Achternaam;Training;Datum training;Subactiviteit;Locatie;Geregistreerd op" };

            rows.ForEach(r => lines.Add($"{CsvField(r.FirstName)};{CsvField(r.LastName)};{CsvField(r.TrainingTitle)};{r.TrainingDate:dd/MM/yyyy};{CsvField(r.SubActivity)};{CsvField(r.Location)};{r.RegisteredAt:dd/MM/yyyy HH:mm}"));

            var csv = string.Join("\r\n", lines);
            var bom = Encoding.UTF8.GetPreamble();
            var content = bom.Concat(Encoding.UTF8.GetBytes(csv)).ToArray();
            var fileName = $"export_{DateTime.Today:yyyy-MM-dd}.csv";

            return File(content, "text/csv", fileName);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Audit()
        {
            var logs = _context.AuditLogs
                .OrderByDescending(l => l.Timestamp)
                .ToList();

            LoadSettings();
            return View("Audit", logs);
        }

        private static string CsvField(string value)
        {
            if (string.IsNullOrEmpty(value)) return "\"\"";
            if (value.StartsWith('=') || value.StartsWith('+') || value.StartsWith('-') || value.StartsWith('@'))
                value = "'" + value;
            return "\"" + value.Replace("\"", "\"\"") + "\"";
        }

        // ACCOUNT MANAGEMENT

        // GET: Admin/Users
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> Users()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            LoadSettings();

            var viewModels = new List<AdminUserViewModel>();
            foreach (var user in allUsers)
            {
                var roles = await _userManager.GetRolesAsync(user);
                viewModels.Add(new AdminUserViewModel
                {
                    User = user,
                    Role = roles.FirstOrDefault() ?? "Member"
                });
            }

            return View(viewModels);
        }

        // POST: Admin/ChangeRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> ChangeRole(string userId, string newRole)
        {
            if (newRole != "Admin" && newRole != "Member")
            {
                TempData["ErrorMessage"] = "Ongeldige rol.";
                return RedirectToAction("Users");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Gebruiker niet gevonden.";
                return RedirectToAction("Users");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Contains("SuperAdmin"))
            {
                TempData["ErrorMessage"] = "De rol van de SuperAdmin kan niet worden aangepast.";
                return RedirectToAction("Users");
            }

            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, newRole);

            TempData["SuccessMessage"] = $"Rol van '{user.Email}' is gewijzigd naar '{newRole}'.";
            return RedirectToAction("Users");
        }

        // POST: Admin/ResetUserPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")] // I have secured this endpoint!
        public async Task<IActionResult> ResetUserPassword(string userId, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                // I validate that the new password is not empty
                TempData["ErrorMessage"] = "Het nieuwe wachtwoord mag niet leeg zijn.";
                return RedirectToAction("Users");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // I handle the case where the user cannot be found
                TempData["ErrorMessage"] = "Gebruiker kon niet worden gevonden.";
                return RedirectToAction("Users");
            }

            var removeResult = await _userManager.RemovePasswordAsync(user);
            if (!removeResult.Succeeded)
            {
                // I ensure the old password is clear before setting a new one
                TempData["ErrorMessage"] = "Het oude wachtwoord kon niet worden gewist.";
                return RedirectToAction("Users");
            }

            var addResult = await _userManager.AddPasswordAsync(user, newPassword);
            if (addResult.Succeeded)
            {
                var expiryInfo = _context.PasswordExpiries.FirstOrDefault(p => p.UserId == user.Id);
                if (expiryInfo != null)
                {
                    expiryInfo.LastPasswordChanged = DateTime.Now;
                    expiryInfo.IsBlockedDueToExpiry = false;
                }
                else
                {
                    _context.PasswordExpiries.Add(new PasswordExpiry
                    {
                        UserId = user.Id,
                        LastPasswordChanged = DateTime.Now,
                        IsBlockedDueToExpiry = false
                    });
                }
                await _context.SaveChangesAsync();
                // I notify the administrator of the successful password update
                TempData["SuccessMessage"] = $"Het wachtwoord voor '{user.Email}' is succesvol gewijzigd naar '{newPassword}'!";
            }
            else
            {
                // I capture and display any errors that occurred during the process
                TempData["ErrorMessage"] = "Fout: " + addResult.Errors.FirstOrDefault()?.Description;
            }

            return RedirectToAction("Users");
        }

        // POST: Admin/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")] // I have secured this endpoint!
        public async Task<IActionResult> CreateUser(string firstname, string lastName, string email, string password)
        {
            string fullName = $"{firstname.ToLower()}.{lastName.ToLower()}";

            if (string.IsNullOrWhiteSpace(firstname) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                // I ensure all registration fields are provided
                TempData["ErrorMessage"] = "Naam, e-mailadres en wachtwoord zijn verplicht.";
                return RedirectToAction("Users");
            }

            var user = new IdentityUser { UserName = fullName, Email = email };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                _context.PasswordExpiries.Add(new PasswordExpiry
                {
                    UserId = user.Id,
                    LastPasswordChanged = DateTime.Now,
                    IsBlockedDueToExpiry = false
                });
                await _context.SaveChangesAsync();
                // I automatically assign the "Admin" role so this user actually gets admin permissions!
                // I assign new users to the standard Admin role by default
                await _userManager.AddToRoleAsync(user, "Admin");
                TempData["SuccessMessage"] = $"Bestuurslid '{firstname} {lastName}' is succesvol toegevoegd!";
            }
            else
            {
                // I log creation failures to inform the administrator
                TempData["ErrorMessage"] = "Fout bij aanmaken: " + result.Errors.FirstOrDefault()?.Description;
            }

            return RedirectToAction("Users");
        }

        // POST: Admin/EditUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")] // I have secured this endpoint!
        public async Task<IActionResult> EditUser(string userId, string newEmail, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
            {
                // I block updates with missing email addresses
                TempData["ErrorMessage"] = "E-mailadres mag niet leeg zijn.";
                return RedirectToAction("Users");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // I verify that the target user actually exists
                TempData["ErrorMessage"] = "Gebruiker kon niet worden gevonden.";
                return RedirectToAction("Users");
            }

            if (user.Email != newEmail)
            {
                var existingEmail = await _userManager.FindByEmailAsync(newEmail);
                if (existingEmail != null)
                {
                    // I prevent email duplication across different accounts
                    TempData["ErrorMessage"] = "Dit e-mailadres is al in gebruik door een ander account.";
                    return RedirectToAction("Users");
                }

                user.Email = newEmail;

                var emailResult = await _userManager.UpdateAsync(user);
                if (!emailResult.Succeeded)
                {
                    // I flag any database update errors for the email address
                    TempData["ErrorMessage"] = "Fout bij het bijwerken van het e-mailadres: " + emailResult.Errors.FirstOrDefault()?.Description;
                    return RedirectToAction("Users");
                }
            }

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                var removeResult = await _userManager.RemovePasswordAsync(user);
                if (removeResult.Succeeded)
                {
                    var addResult = await _userManager.AddPasswordAsync(user, newPassword);
                    if (!addResult.Succeeded)
                    {
                        // I warn the admin if the email changed but password update failed
                        TempData["ErrorMessage"] = "E-mail is aangepast, maar wachtwoord wijzigen mislukt: " + addResult.Errors.FirstOrDefault()?.Description;
                        return RedirectToAction("Users");
                    }
                    var expiryInfo = _context.PasswordExpiries.FirstOrDefault(p => p.UserId == user.Id);
                    if (expiryInfo != null)
                    {
                        expiryInfo.LastPasswordChanged = DateTime.Now;
                        expiryInfo.IsBlockedDueToExpiry = false;
                        await _context.SaveChangesAsync();
                    }
                }
            }

            // I confirm the final successful updates to the admin UI
            TempData["SuccessMessage"] = $"Gegevens voor '{newEmail}' succesvol bijgewerkt!";
            return RedirectToAction("Users");
        }

        // POST: Admin/DeleteUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SuperAdmin")] // I have secured this endpoint!
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // I stop execution if the user to delete doesn't exist
                TempData["ErrorMessage"] = "Gebruiker niet gevonden.";
                return RedirectToAction("Users");
            }

            if (user.UserName == User.Identity?.Name || user.Email == User.Identity?.Name)
            {
                // I explicitly prevent administrators from accidentally deleting themselves
                TempData["ErrorMessage"] = "Je kunt je eigen administrator-account niet verwijderen!";
                return RedirectToAction("Users");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                var expiryInfo = _context.PasswordExpiries.FirstOrDefault(p => p.UserId == user.Id);
                if (expiryInfo != null)
                {
                    _context.PasswordExpiries.Remove(expiryInfo);
                    await _context.SaveChangesAsync();
                }
                // I confirm the final permanent deletion
                TempData["SuccessMessage"] = $"Account '{user.Email}' is succesvol permanent verwijderd.";
            }
            else
            {
                // I fall back to a failure error message if database deletion fails
                TempData["ErrorMessage"] = "Gebruiker verwijderen mislukt.";
            }

            return RedirectToAction("Users");
        }

        // Add this action at the bottom of your AdminController.cs (right below Account Management)

        // GET: Admin/Logs
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")] // I restrict this highly sensitive diagnostic page exclusively to SuperAdmins
        public async Task<IActionResult> Logs(string? logLevel, string? search, int page = 1)
        {
            if (page < 1) page = 1;
            int pageSize = 20; // I set a reasonable page size to ensure the dashboard remains fast and responsive

            // I initialize the query against the SystemLogs table
            var query = _context.SystemLogs.AsQueryable();

            // I filter by LogLevel if the SuperAdmin selected a specific severity level
            if (!string.IsNullOrWhiteSpace(logLevel))
            {
                query = query.Where(l => l.LogLevel == logLevel);
            }

            // I apply a text search filter across message, exception, and user fields if requested
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(l => l.Message.Contains(search)
                                      || (l.Exception != null && l.Exception.Contains(search))
                                      || (l.Username != null && l.Username.Contains(search)));
            }

            // I order the logs chronologically descending so that the newest technical issues show up first
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pagedLogs = await query
                .OrderByDescending(l => l.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // I store the pagination and filter values in the ViewBag to preserve state in the HTML View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages == 0 ? 1 : totalPages;
            ViewBag.LogLevel = logLevel;
            ViewBag.Search = search;

            LoadSettings();

            // I return the list of logs directly to the specialized Admin logs view
            return View("~/Views/Admin/Logs.cshtml", pagedLogs);
        }
    }
}
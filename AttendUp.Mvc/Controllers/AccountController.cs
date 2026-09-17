using AttendUp.Mvc.Data;
using AttendUp.Mvc.Models;
using AttendUp.Mvc.Services.Interfaces;
using AttendUp.Mvc.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AttendUp.Mvc.Controllers
{
    public class AccountController : BaseController
    {
        // I store the Microsoft Identity managers here so I can use them throughout the entire controller.
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;
        // I declare the logger instance to capture authentication-related events
        private readonly ILogger<AccountController> _logger;
        // I update the constructor to request the ILogger instance via ASP.NET Dependency Injection
        public AccountController(
            AttendUpContext context,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IEmailService emailService,
            ILogger<AccountController> logger) : base(context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailService = emailService;
            _logger = logger; // I assign the injected logger to my private field
        }

        // GET: I show the default login screen to the visitor.
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin") || User.IsInRole("SuperAdmin"))
                    return RedirectToAction("Index", "Admin");
                return RedirectToAction("Index", "Registration");
            }

            LoadSettings();
            return View();
        }

        // POST: I process the login credentials entered by the user.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // I first check if the form is completely and correctly filled out according to the model.
            if (!ModelState.IsValid)
            {
                LoadSettings();
                return View(model);
            }

            // 1. SOLUTION: I first search the database for the entered email address.
            var user = await _userManager.FindByEmailAsync(model.Email);

            // 2. If I find a user with that email address...
            if (user != null)
            {
                var expiryInfo = _context.PasswordExpiries.FirstOrDefault(p => p.UserId == user.Id);

                if (expiryInfo != null)
                {
                    bool isExpired = expiryInfo.LastPasswordChanged.AddMonths(6) < DateTime.Now;

                    if (expiryInfo.IsBlockedDueToExpiry || isExpired)
                    {
                        if (!expiryInfo.IsBlockedDueToExpiry)
                        {
                            expiryInfo.IsBlockedDueToExpiry = true;
                            await _context.SaveChangesAsync();
                        }

                        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                        var callbackUrl = Url.Action("ResetPassword", "Account",
                            new { token = token, email = user.Email },
                            protocol: Request.Scheme);

                        await _emailService.SendAsync(
                            model.Email,
                            "Password Reset Required",
                            $"Je wachtwoord is langer dan 6 maanden oud en moet worden gewijzigd:<br><br>" +
                            $"<a href='{callbackUrl}'>Klik hier om je wachtwoord te resetten</a>");

                        LoadSettings();
                        ModelState.AddModelError(string.Empty, "Je wachtwoord is verlopen. Er is een herstellink gestuurd naar je e-mailadres.");
                        return View(model);
                    }
                }
                // 3. ...then I log them in using their actual UserName (whatever is in that field!).
                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: true); // I change this to true to support security lockouts on suspicious behavior

                if (result.Succeeded)
                {
                    // I log a successful authentication event with structured parameters
                    _logger.LogInformation("I successfully authenticated user {Email} from IP {IP}", model.Email, HttpContext.Connection.RemoteIpAddress);
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Admin") || roles.Contains("SuperAdmin"))
                        return RedirectToAction("Index", "Admin");

                    return RedirectToAction("Index", "Registration");
                }
            }
            // I log a warning because an invalid login attempt might indicate suspicious activity
            _logger.LogWarning("I detected a failed login attempt for email: {Email} from IP: {IP}", model.Email, HttpContext.Connection.RemoteIpAddress);

            // Als het inloggen mislukt, voeg ik deze foutmelding toe die de HTML op het scherm kan tonen.
            LoadSettings();
            ModelState.AddModelError(string.Empty, "Ongeldige loginpoging.");
            return View(model);
        }

        // GET: I show the registration form to new visitors.
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Registration");

            LoadSettings();
            return View();
        }

        // POST: Create a new account and assign the Member role.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadSettings();
                return View(model);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Dit e-mailadres is al in gebruik.");
                LoadSettings();
                return View(model);
            }

            var user = new IdentityUser
            {
                UserName = $"{model.FirstName}.{model.LastName}",
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Registration");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            LoadSettings();
            return View(model);
        }

                // GET: Show account settings with the current first/last name pre-filled.
        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login");

            var parts = user.UserName?.Split('.') ?? [];
            var model = new AccountSettingsViewModel
            {
                FirstName = parts.Length > 0 ? parts[0] : string.Empty,
                LastName = parts.Length > 1 ? string.Join(".", parts[1..]) : string.Empty
            };

            LoadSettings();
            return View(model);
        }

        // POST: Save the updated first/last name as the new UserName.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(AccountSettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadSettings();
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login");

            var newUserName = $"{model.FirstName}.{model.LastName}";
            var result = await _userManager.SetUserNameAsync(user, newUserName);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                ViewBag.Success = "Je gegevens zijn opgeslagen.";
                LoadSettings();
                return View(model);
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            LoadSettings();
            return View(model);
        }

        // POST: Delete the logged-in user's own account. Admins and SuperAdmins are not allowed.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login");

            if (await _userManager.IsInRoleAsync(user, "Admin") || await _userManager.IsInRoleAsync(user, "SuperAdmin"))
                return Forbid();

            await _signInManager.SignOutAsync();
            await _userManager.DeleteAsync(user);

            return RedirectToAction("Login");
        }

        // POST: I properly log out the current user and destroy the cookie session.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // I capture who is logging out before destroying the session cookie
            _logger.LogInformation("I am logging out user: {User}", User.Identity?.Name);

            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        // GET: I show the "Forgot Password" page where you can fill in your email address.
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            LoadSettings();
            return View();
        }

        // POST: I capture the filled-in ViewModel and generate the secret reset link.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadSettings();
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = Url.Action("ResetPassword", "Account",
                    new { token = token, email = user.Email },
                    protocol: Request.Scheme);

                ViewBag.Confirm = $"WACHTWOORDRESET VERZONDEN NAAR HET E-MAILADRES: {model.Email}";

                await _emailService.SendAsync(
                    model.Email,
                    "Password Reset",
                    $"Er is een wachtwoordreset aangevraagd voor dit e-mailadres:<br><br>" +
                    $"<a href='{callbackUrl}'>Klik hier om je wachtwoord te resetten</a>");
            }
            else
            {
                ViewBag.Foutmelding = $"Het e-mailadres '{model.Email}' staat niet in de database! Typ het admin-adres goed over, of maak eerst een user aan.";
            }

            // Ik kies ervoor om de gebruiker op exact dezelfde pagina te laten blijven zodat we de link live kunnen zien.
            LoadSettings();
            return View(model);
        }

        // GET: I show the page where the user can type their new password.
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                return RedirectToAction("Login");
            }

            // Ik stop de token en de e-mail in het model zodat ze onzichtbaar in de HTML meereizen.
            LoadSettings();
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        // POST: I process the new password and update it in the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadSettings();
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // BUGFIX: I restore any potential spaces in the token back to plus signs.
            var result = await _userManager.ResetPasswordAsync(user, model.Token.Replace(" ", "+"), model.Password);

            if (result.Succeeded)
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

                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            LoadSettings();
            return View(model);
        }
    }
}
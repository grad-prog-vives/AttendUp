using AttendUp.Mvc.Data;
using AttendUp.Mvc.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AttendUp.Mvc.Controllers
{
    public class BaseController : Controller
    {
        protected readonly AttendUpContext _context;

        public BaseController(AttendUpContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var currentController = context.RouteData.Values["controller"]?.ToString();
                var currentAction = context.RouteData.Values["action"]?.ToString();

                if (currentController == "Account" && (currentAction == "Login" || currentAction == "ResetPassword" || currentAction == "Logout"))
                {
                    return;
                }

                var userManager = context.HttpContext.RequestServices.GetService<UserManager<IdentityUser>>();
                var signInManager = context.HttpContext.RequestServices.GetService<SignInManager<IdentityUser>>();

                if (userManager != null && signInManager != null)
                {
                    var user = userManager.FindByNameAsync(User.Identity.Name).Result;

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
                                    _context.SaveChanges();
                                }
                                signInManager.SignOutAsync().Wait();

                                context.Result = new RedirectToActionResult("Login", "Account", null);
                            }
                        }
                    }
                }
            }
        }

        // Loads the branding settings from the database and stores them in the ViewBag. If no branding exists, an empty Branding instance is used
        protected void LoadSettings()
        {
            var branding = _context.Settings.FirstOrDefault() ?? new Setting();

            // Huidige dag en maand
            int day = DateTime.Today.Day;
            int month = DateTime.Today.Month;

            //OM TE TESTEN (DEMO)

            //// HALLOWEEN
            //int day = 15;
            //int month = 10;

            //// KERST
            //int day = 15;
            //int month = 12;

            //// PASEN
            // int day = 16;
            // int month = 4;


            var display = new Setting
            {
                ID = branding.ID,
                LogoPath = branding.LogoPath,
                DefaultButtonColor = branding.DefaultButtonColor,
                DefaultBackgroundColor = branding.DefaultBackgroundColor,
                EventBannerPath = branding.EventBannerPath
            };

            // Kerst: 1 tot en met 26 december
            if (month == 12 && day >= 1 && day <= 26)
            {
                display.ButtonColor = "#c0392b";
                display.BackgroundColor = "#1a472a";
            }
            // Halloween: hele maand oktober
            else if (month == 10)
            {
                display.ButtonColor = "#ff6600";
                display.BackgroundColor = "#1a1a1a";
            }
            // Pasen: 14 tot en met 21 april
            else if (month == 4 && day >= 14 && day <= 21)
            {
                display.ButtonColor = "#bcc7f4";
                display.BackgroundColor = "#e8df62";
            }
            // Reset naar de ingestelde standaardkleuren indien niks
            else
            {
                display.ButtonColor = branding.ButtonColor;
                display.BackgroundColor = branding.BackgroundColor;
            }

            // Geen SaveChanges() meer hier, alleen voor weergave
            ViewBag.Branding = display;
        }
        protected void LogAudit(string action, string details)
        {
            string user = "Onbekende Admin";

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var rawName = User.Identity.Name;

                if (!string.IsNullOrEmpty(rawName))
                {
                    if (rawName.Contains('@'))
                    {
                        rawName = rawName.Split('@')[0];
                    }

                    if (rawName.Contains('.'))
                    {
                        var parts = rawName.Split('.');
                        user = string.Join(" ", parts.Select(p => char.ToUpper(p[0]) + p.Substring(1)));
                    }
                    else
                    {
                        user = char.ToUpper(rawName[0]) + rawName.Substring(1);
                    }
                }
            }

            var log = new AuditLog
            {
                User = user,
                Action = action,
                Details = details,
                Timestamp = DateTime.Now
            };

            _context.AuditLogs.Add(log);
        }
    }
}

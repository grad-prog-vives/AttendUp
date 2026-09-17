using AttendUp.Mvc.Data;
using AttendUp.Mvc.Models;
using AttendUp.Mvc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AttendUp.Mvc.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class LocationController : Controller
    {
        private readonly AttendUpContext _context;

        public LocationController(AttendUpContext context)
        {
            _context = context;
        }

        [HttpGet("Admin/Location/Create")]
        public IActionResult LocationCreate()
        {
            return View("~/Views/Admin/LocationCreate.cshtml", new Location());
        }

        [HttpPost("Admin/Location/Create")]
        [ValidateAntiForgeryToken]
        public IActionResult LocationCreate(Location model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/LocationCreate.cshtml", model);

            var exists = _context.Locations.Any(l => l.Name.ToLower() == model.Name.ToLower());
            if (exists)
            {
                ModelState.AddModelError("Name", "Deze locatie bestaat al.");
                return View("~/Views/Admin/LocationCreate.cshtml", model);
            }

            model.IsActive = true;
            _context.Locations.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Location", "Admin");
        }

        [HttpGet("Admin/Location/Edit/{id}")]
        public IActionResult LocationEdit(int id)
        {
            var location = _context.Locations.Find(id);
            if (location == null) return NotFound();
            return View("~/Views/Admin/LocationEdit.cshtml", location);
        }

        [HttpPost("Admin/Location/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult LocationEdit(Location model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/Admin/LocationEdit.cshtml", model);

            var exists = _context.Locations.Any(l => l.Id != model.Id && l.Name.ToLower() == model.Name.Trim().ToLower());
            if (exists)
            {
                ModelState.AddModelError("Name", "Deze locatie bestaat al.");
                return View("~/Views/Admin/LocationEdit.cshtml", model);
            }

            var location = _context.Locations.Find(model.Id);
            if (location == null) return NotFound();

            location.Name = model.Name;
            _context.SaveChanges();

            return RedirectToAction("Location", "Admin");
        }

        [HttpGet("Admin/Location/QrCode/{id}")]
        public IActionResult QrCode(int id)
        {
            var location = _context.Locations.Find(id);
            if (location == null) return NotFound();

            var url = $"{Request.Scheme}://{Request.Host}/Registration?locationId={id}";
            var png = GeneratePng(url);

            var vm = new LocationQrCodeViewModel
            {
                LocationId = id,
                LocationName = location.Name,
                Url = url,
                QrCodeBase64 = Convert.ToBase64String(png)
            };

            return View("~/Views/Admin/LocationQrCode.cshtml", vm);
        }

        [HttpGet("Admin/Location/QrCodeDownload/{id}")]
        public IActionResult QrCodeDownload(int id, string format)
        {
            var location = _context.Locations.Find(id);
            if (location == null) return NotFound();

            var url = $"{Request.Scheme}://{Request.Host}/Registration?locationId={id}";
            var fileName = $"qrcode-{location.Name.ToLower().Replace(" ", "-")}";
            var png = GeneratePng(url);

            if (format == "pdf")
            {
                QuestPDF.Settings.License = LicenseType.Community;

                var pdf = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.Content().Column(col =>
                        {
                            col.Item().AlignCenter().Text(location.Name).FontSize(24).Bold();
                            col.Item().Height(20);
                            col.Item().AlignCenter().Image(png);
                        });
                    });
                }).GeneratePdf();

                return File(pdf, "application/pdf", $"{fileName}.pdf");
            }

            return File(png, "image/png", $"{fileName}.png");
        }

        private byte[] GeneratePng(string url)
        {
            using var qrGenerator = new QRCodeGenerator();
            using var qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrData);
            return qrCode.GetGraphic(10);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LocationToggle(int id)
        {
            var location = _context.Locations.Find(id);
            if (location == null) return NotFound();

            location.IsActive = !location.IsActive;
            _context.SaveChanges();

            return RedirectToAction("Location", "Admin");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LocationDelete(int id)
        {
            var location = _context.Locations.Find(id);
            if (location == null) return NotFound();

            var hasRegistrations = _context.Registrations.Any(r => r.LocationId == id);
            if (hasRegistrations)
            {
                TempData["ErrorMessage"] = "Kan niet verwijderen: deze locatie wordt nog gebruikt in registraties.";
                return RedirectToAction("Location", "Admin");
            }

            _context.Locations.Remove(location);
            _context.SaveChanges();

            return RedirectToAction("Location", "Admin");
        }
    }
}

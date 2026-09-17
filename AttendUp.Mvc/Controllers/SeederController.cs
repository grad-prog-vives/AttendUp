using AttendUp.Mvc.Data;
using AttendUp.Mvc.Data.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace AttendUp.Mvc.Controllers
{
    public class SeederController : Controller
    {
        private readonly AttendUpContext _context;
        public SeederController(AttendUpContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                await DbSeeder.SeedRegistrationsAsync(_context);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;

            }
            return View();

        }
    }
}

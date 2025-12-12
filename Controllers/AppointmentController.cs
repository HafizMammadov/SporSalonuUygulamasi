using Microsoft.AspNetCore.Mvc;

namespace SporSalonuUygulamasi.Controllers
{
    public class AppointmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

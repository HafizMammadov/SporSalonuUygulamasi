using Microsoft.AspNetCore.Mvc;

namespace SporSalonuUygulamasi.Controllers
{
    public class TrainersApiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

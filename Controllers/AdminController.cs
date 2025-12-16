using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SporSalonuUygulamasi.Utility;

namespace SporSalonuUygulamasi.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

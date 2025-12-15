using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // Yetkilendirme (Authorize) için eklendi!
using SporSalonuUygulamasi.Models;
using Microsoft.Extensions.Logging; // Logger'ýn düzgün çalýþmasý için (zaten vardý)

namespace SporSalonuUygulamasi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // 1. Anasayfa (Index) - BURASI GÜNCELLENDÝ
        public IActionResult Index()
        {
            // Kullanýcý giriþ yapmýþsa (kimliði doðrulanmýþsa) Dashboard'a yönlendir.
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }

            // Giriþ yapmamýþsa, Login/Register sayfasýna yönlendir.
            return RedirectToAction("LoginRegister", "Account");
        }

        // 2. Dashboard Action'ý - YENÝ EKLENDÝ
        [Authorize] // Sadece giriþ yapmýþ kullanýcýlar eriþebilir
        public IActionResult Dashboard()
        {
            // Views/Home/Dashboard.cshtml dosyasýný görüntüleyecektir.
            return View();
        }

        // 3. Admin Paneli - YENÝ EKLENDÝ (Rol Bazlý Yetkilendirme)
        
        [Authorize(Roles = "Admin")] // Sadece "Admin" rolüne sahip kullanýcýlar eriþebilir. [cite: 49]
        public IActionResult AdminPanel()
        {
            // Views/Home/AdminPanel.cshtml dosyasýný görüntüleyecektir.
            return View();
        }

        // Mevcut Privacy ve Error metodlarý aþaðýda kaldý.
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

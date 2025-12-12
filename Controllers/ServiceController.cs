using SporSalonuUygulamasi.Models;
using Microsoft.AspNetCore.Mvc;
using SporSalonuUygulamasi.Data;
using System.Linq;

namespace SporSalonuUygulamasi.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServiceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. LİSTELEME SAYFASI
        public IActionResult Index()
        {
            var services = _context.Services.ToList();
            return View(services);
        }

        // 2. EKLEME SAYFASI (Formu Gösterir)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // 3. EKLEME İŞLEMİ (Veriyi Kaydeder)
        [HttpPost]
        public IActionResult Create(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Services.Add(service);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(service);
        }
    }
}
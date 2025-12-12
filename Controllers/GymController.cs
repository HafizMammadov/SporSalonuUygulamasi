using Microsoft.AspNetCore.Mvc;
using SporSalonuUygulamasi.Data;
using SporSalonuUygulamasi.Models;
using System.Linq;

namespace SporSalonuUygulamasi.Controllers
{
    public class GymController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GymController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Salonları Listele
        public IActionResult Index()
        {
            var gyms = _context.Gyms.ToList();
            return View(gyms);
        }

        // Yeni Salon Ekleme Sayfası
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Yeni Salonu Kaydet
        [HttpPost]
        public IActionResult Create(Gym gym)
        {
            if (ModelState.IsValid)
            {
                _context.Gyms.Add(gym);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gym);
        }
    }
}
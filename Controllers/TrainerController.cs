using SporSalonuUygulamasi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Dropdown için gerekli
using Microsoft.EntityFrameworkCore;
using SporSalonuUygulamasi.Data;
using System.Linq;

namespace SporSalonuUygulamasi.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainerController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Include ile ilişkili Salon ve Hizmet verilerini de getiriyoruz
            var trainers = _context.Trainers
                                   .Include(t => t.Gym)
                                   .Include(t => t.Service)
                                   .ToList();
            return View(trainers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            // Sayfa açılırken Dropdown'ları dolduruyoruz
            ViewBag.Gyms = new SelectList(_context.Gyms.ToList(), "GymId", "Name");
            ViewBag.Services = new SelectList(_context.Services.ToList(), "ServiceId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Trainer trainer)
        {
            // Dropdown verilerini tekrar yüklüyoruz (Hata olursa kaybolmasın diye)
            ViewBag.Gyms = new SelectList(_context.Gyms.ToList(), "GymId", "Name");
            ViewBag.Services = new SelectList(_context.Services.ToList(), "ServiceId", "Name");

            if (ModelState.IsValid)
            {
                _context.Trainers.Add(trainer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(trainer);
        }
    }
}
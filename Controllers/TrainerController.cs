using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Dropdown için şart
using Microsoft.EntityFrameworkCore; // Include için şart
using SporSalonuUygulamasi.Data;
using SporSalonuUygulamasi.Models;
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

        // LİSTELEME
        public IActionResult Index()
        {
            // Eğitmenleri listelerken Salonunu ve Hizmetini de getir
            var trainers = _context.Trainers
                                   .Include(t => t.Gym)
                                   .Include(t => t.Service)
                                   .ToList();
            return View(trainers);
        }

        // EKLEME SAYFASI (GET)
        [HttpGet]
        public IActionResult Create()
        {
            // 1. Salonları Dropdown için hazırla
            ViewBag.GymList = new SelectList(_context.Gyms.ToList(), "GymId", "Name");

            // 2. Hizmetleri Dropdown için hazırla
            ViewBag.ServiceList = new SelectList(_context.Services.ToList(), "ServiceId", "Name");

            return View();
        }

        // EKLEME İŞLEMİ (POST)
        [HttpPost]
        public IActionResult Create(Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                _context.Trainers.Add(trainer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Hata olursa (örn: boş alan) listeleri tekrar yükle
            ViewBag.GymList = new SelectList(_context.Gyms.ToList(), "GymId", "Name");
            ViewBag.ServiceList = new SelectList(_context.Services.ToList(), "ServiceId", "Name");

            return View(trainer);
        }

        // SİLME İŞLEMİ
        public IActionResult Delete(int id)
        {
            var trainer = _context.Trainers.Find(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
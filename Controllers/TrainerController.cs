using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SporSalonuUygulamasi.Data;
using SporSalonuUygulamasi.Models;
using System.Linq;

namespace SporSalonuUygulamasi.Controllers
{
    [Authorize]
    public class TrainerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ana menü sayfası
        public IActionResult Index()
        {
            return View();
        }

        // Eğitmenleri listele
        public IActionResult List()
        {
            var trainers = _context.Trainers.Include(t => t.Gym).ToList();
            return View(trainers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.GymList = new SelectList(_context.Gyms.ToList(), "GymId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Trainer trainer, string acilisSaati, string kapanisSaati)
        {
            // Çalışma saatleri kontrolü
            if (string.IsNullOrEmpty(acilisSaati) || string.IsNullOrEmpty(kapanisSaati))
            {
                ModelState.AddModelError("WorkingHours", "Lütfen çalışma saatlerini giriniz!");
            }
            else
            {
                trainer.WorkingHours = $"{acilisSaati} - {kapanisSaati}";
            }

            if (ModelState.IsValid)
            {
                _context.Trainers.Add(trainer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GymList = new SelectList(_context.Gyms.ToList(), "GymId", "Name", trainer.GymId);
            ViewBag.Acilis = acilisSaati;
            ViewBag.Kapanis = kapanisSaati;
            return View(trainer);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var trainer = _context.Trainers.Find(id);
            if (trainer == null) return NotFound();

            ViewBag.GymList = new SelectList(_context.Gyms.ToList(), "GymId", "Name", trainer.GymId);

            if (!string.IsNullOrEmpty(trainer.WorkingHours) && trainer.WorkingHours.Contains("-"))
            {
                var saatler = trainer.WorkingHours.Split('-');
                ViewBag.Acilis = saatler[0].Trim();
                ViewBag.Kapanis = saatler[1].Trim();
            }

            return View(trainer);
        }

        [HttpPost]
        public IActionResult Edit(int id, Trainer trainer, string acilisSaati, string kapanisSaati)
        {
            trainer.TrainerId = id;
            
            // Çalışma saatleri kontrolü
            if (string.IsNullOrEmpty(acilisSaati) || string.IsNullOrEmpty(kapanisSaati))
            {
                ModelState.AddModelError("WorkingHours", "Lütfen çalışma saatlerini giriniz!");
            }
            else
            {
                trainer.WorkingHours = $"{acilisSaati} - {kapanisSaati}";
            }

            if (ModelState.IsValid)
            {
                _context.Trainers.Update(trainer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GymList = new SelectList(_context.Gyms.ToList(), "GymId", "Name", trainer.GymId);
            ViewBag.Acilis = acilisSaati;
            ViewBag.Kapanis = kapanisSaati;

            return View(trainer);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var trainer = _context.Trainers.Find(id);
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                _context.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}


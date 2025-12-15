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

        // LİSTELEME
        public IActionResult Index()
        {
            return View(_context.Gyms.ToList());
        }

        // EKLEME SAYFASI (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // EKLEME İŞLEMİ (POST)
        [HttpPost]
        public IActionResult Create(Gym gym, string acilisSaati, string kapanisSaati)
        {
            // 1. ZORUNLULUK KONTROLÜ
            if (string.IsNullOrEmpty(acilisSaati) || string.IsNullOrEmpty(kapanisSaati))
            {
                ModelState.AddModelError("WorkingHours", "Lütfen açılış ve kapanış saatlerini giriniz!");
            }
            else
            {
                gym.WorkingHours = $"{acilisSaati} - {kapanisSaati}";
            }

            // 2. KAYIT
            if (ModelState.IsValid)
            {
                _context.Gyms.Add(gym);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gym);
        }
        // DÜZENLEME SAYFASI (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var gym = _context.Gyms.Find(id);
            if (gym == null) return NotFound();

            // Veritabanındaki "09:00 - 22:00" formatını parçalayıp View'a gönderelim
            // Böylece kutucuklar dolu gelir.
            if (!string.IsNullOrEmpty(gym.WorkingHours) && gym.WorkingHours.Contains("-"))
            {
                var saatler = gym.WorkingHours.Split('-');
                ViewBag.Acilis = saatler[0].Trim();
                ViewBag.Kapanis = saatler[1].Trim();
            }

            return View(gym);
        }

        // DÜZENLEME İŞLEMİ (POST)
        [HttpPost]
        public IActionResult Edit(Gym gym, string acilisSaati, string kapanisSaati)
        {
            // 1. ZORUNLULUK KONTROLÜ
            if (string.IsNullOrEmpty(acilisSaati) || string.IsNullOrEmpty(kapanisSaati))
            {
                ModelState.AddModelError("WorkingHours", "Lütfen açılış ve kapanış saatlerini giriniz!");
            }
            else
            {
                gym.WorkingHours = $"{acilisSaati} - {kapanisSaati}";
            }

            // 2. GÜNCELLEME
            if (ModelState.IsValid)
            {
                _context.Gyms.Update(gym);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            // Hata olursa kutucuklar boş kalmasın diye geri dolduruyoruz
            ViewBag.Acilis = acilisSaati;
            ViewBag.Kapanis = kapanisSaati;

            return View(gym);
        }
        // SİLME SAYFASI
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var gym = _context.Gyms.Find(id);
            if (gym == null) return NotFound();
            return View(gym);
        }

        // SİLME ONAYI
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var gym = _context.Gyms.Find(id);
            if (gym != null)
            {
                _context.Gyms.Remove(gym);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
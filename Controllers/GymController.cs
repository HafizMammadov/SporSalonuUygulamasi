using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonuUygulamasi.Data;
using SporSalonuUygulamasi.Models;
using System.Linq;

namespace SporSalonuUygulamasi.Controllers
{
    [Authorize]
    public class GymController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GymController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Ana menü sayfası
        public IActionResult Index()
        {
            return View();
        }

        // Salonları listele
        public IActionResult List()
        {
            var gyms = _context.Gyms.Include(g => g.GymServices).ToList();
            return View(gyms);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Gym gym, string acilisSaati, string kapanisSaati, 
            string[] serviceNames, decimal[] hourlyRates)
        {
            // Çalışma saatleri kontrolü
            if (string.IsNullOrEmpty(acilisSaati) || string.IsNullOrEmpty(kapanisSaati))
            {
                ModelState.AddModelError("WorkingHours", "Lütfen açılış ve kapanış saatlerini giriniz!");
            }
            else
            {
                gym.WorkingHours = $"{acilisSaati} - {kapanisSaati}";
            }

            // En az 1 hizmet kontrolü
            if (serviceNames == null || serviceNames.Length == 0 || 
                serviceNames.All(s => string.IsNullOrWhiteSpace(s)))
            {
                ModelState.AddModelError("GymServices", "En az bir hizmet eklemelisiniz!");
            }

            if (ModelState.IsValid)
            {
                _context.Gyms.Add(gym);
                _context.SaveChanges();

                // Hizmetleri ekle
                if (serviceNames != null)
                {
                    for (int i = 0; i < serviceNames.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(serviceNames[i]))
                        {
                            var gymService = new GymService
                            {
                                GymId = gym.GymId,
                                ServiceName = serviceNames[i],
                                HourlyRate = hourlyRates != null && i < hourlyRates.Length ? hourlyRates[i] : 0
                            };
                            _context.GymServices.Add(gymService);
                        }
                    }
                    _context.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            ViewBag.Acilis = acilisSaati;
            ViewBag.Kapanis = kapanisSaati;
            ViewBag.ServiceNames = serviceNames;
            ViewBag.HourlyRates = hourlyRates;

            return View(gym);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var gym = _context.Gyms.Include(g => g.GymServices).FirstOrDefault(g => g.GymId == id);
            if (gym == null) return NotFound();

            if (!string.IsNullOrEmpty(gym.WorkingHours) && gym.WorkingHours.Contains("-"))
            {
                var saatler = gym.WorkingHours.Split('-');
                ViewBag.Acilis = saatler[0].Trim();
                ViewBag.Kapanis = saatler[1].Trim();
            }

            return View(gym);
        }

        [HttpPost]
        public IActionResult Edit(int gymId, Gym gym, string acilisSaati, string kapanisSaati,
            string[] serviceNames, decimal[] hourlyRates, int[] serviceIds)
        {
            gym.GymId = gymId;
            
            // Çalışma saatleri kontrolü
            if (string.IsNullOrEmpty(acilisSaati) || string.IsNullOrEmpty(kapanisSaati))
            {
                ModelState.AddModelError("WorkingHours", "Lütfen açılış ve kapanış saatlerini giriniz!");
            }
            else
            {
                gym.WorkingHours = $"{acilisSaati} - {kapanisSaati}";
            }

            // En az 1 hizmet kontrolü
            if (serviceNames == null || serviceNames.Length == 0 || 
                serviceNames.All(s => string.IsNullOrWhiteSpace(s)))
            {
                ModelState.AddModelError("GymServices", "En az bir hizmet eklemelisiniz!");
            }

            if (ModelState.IsValid)
            {
                _context.Gyms.Update(gym);
                _context.SaveChanges();

                // Mevcut hizmetleri sil
                var existingServices = _context.GymServices.Where(gs => gs.GymId == gymId).ToList();
                _context.GymServices.RemoveRange(existingServices);

                // Yeni hizmetleri ekle
                if (serviceNames != null)
                {
                    for (int i = 0; i < serviceNames.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(serviceNames[i]))
                        {
                            var gymService = new GymService
                            {
                                GymId = gymId,
                                ServiceName = serviceNames[i],
                                HourlyRate = hourlyRates != null && i < hourlyRates.Length ? hourlyRates[i] : 0
                            };
                            _context.GymServices.Add(gymService);
                        }
                    }
                }
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.Acilis = acilisSaati;
            ViewBag.Kapanis = kapanisSaati;
            
            // Hizmetleri tekrar yükle
            gym.GymServices = _context.GymServices.Where(gs => gs.GymId == gymId).ToList();

            return View(gym);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var gym = _context.Gyms.Include(g => g.GymServices).FirstOrDefault(g => g.GymId == id);
            if (gym != null)
            {
                // Önce ilişkili hizmetleri sil
                _context.GymServices.RemoveRange(gym.GymServices);
                _context.Gyms.Remove(gym);
                _context.SaveChanges();
            }
            return RedirectToAction("List");
        }
    }
}
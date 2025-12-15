using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SporSalonuUygulamasi.Data;
using SporSalonuUygulamasi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity; // EKLENDİ: Kullanıcı yönetimi için
using System.Security.Claims;      // EKLENDİ: Kullanıcı ID'sini almak için
using Microsoft.AspNetCore.Authorization; // EKLENDİ: Yetkilendirme için

namespace SporSalonuUygulamasi.Controllers
{
    // Controller seviyesinde yetkilendirme yok, metotlara özel yetkilendirme var.
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager; // EKLENDİ: Kullanıcı yöneticisi

        public AppointmentController(ApplicationDbContext context, UserManager<AppUser> userManager) // GÜNCELLENDİ: UserManager eklendi
        {
            _context = context;
            _userManager = userManager;
        }

        // ==========================================
        // 1. LİSTELEME SAYFASI (INDEX) - SADECE YÖNETİCİ
        // ==========================================
        [Authorize(Roles = "Admin")] // SADECE YÖNETİCİ TÜM LİSTEYİ GÖREBİLİR
        public async Task<IActionResult> Index()
        {
            var appointments = _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .OrderByDescending(a => a.AppointmentDate)
                .AsQueryable();

            return View(await appointments.ToListAsync());
        }

        // ==========================================
        // 2. RANDEVU ALMA SAYFASINI AÇ (GET) - GİRİŞ ŞARTI
        // ==========================================
        [Authorize] // GİRİŞ YAPAN HERKES RANDEVU ALABİLİR
        public IActionResult Create()
        {
            // Hizmetleri Doldur
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name");

            // Eğitmenleri Doldur
            var trainers = _context.Trainers.Select(t => new
            {
                Id = t.Id,
                FullName = t.FirstName + " " + t.LastName
            }).ToList();

            ViewData["TrainerId"] = new SelectList(trainers, "Id", "FullName");

            return View();
        }

        // ==========================================
        // 3. RANDEVUYU KAYDET (POST) - KULLANICI ID'SİNİ EKLEME
        // ==========================================
        [Authorize] // GİRİŞ YAPAN HERKES RANDEVU ALABİLİR
        [HttpPost]
        [ValidateAntiForgeryToken]
        // KULLANICI ID'si modelde olmadığı için Bind'e eklenmedi. Manuel ekleyeceğiz.
        public async Task<IActionResult> Create([Bind("Id,AppointmentDate,TrainerId,ServiceId")] Appointment appointment)
        {
            // Oturum açan kullanıcının ID'sini alıyoruz
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            appointment.UserId = userId; // KRİTİK: Randevuyu alan kullanıcının ID'sini kaydettik.

            if (ModelState.IsValid)
            {
                // Varsayılan olarak IsConfirmed = false (Beklemede) kaydedilir.
                _context.Add(appointment);
                await _context.SaveChangesAsync();

                // Başarılı kayıttan sonra kullanıcının kendi randevu listesine yönlendir
                return RedirectToAction(nameof(MyAppointments));
            }

            // Hata olursa listeleri tekrar doldur
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);

            var trainers = _context.Trainers.Select(t => new
            {
                Id = t.Id,
                FullName = t.FirstName + " " + t.LastName
            }).ToList();

            ViewData["TrainerId"] = new SelectList(trainers, "Id", "FullName", appointment.TrainerId);

            return View(appointment);
        }

        // ==========================================
        // 4. RANDEVUYU ONAYLA - SADECE YÖNETİCİ
        // ==========================================
        [Authorize(Roles = "Admin")] // SADECE YÖNETİCİ ONAYLAYABİLİR
        public async Task<IActionResult> Approve(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            // Durumu Onaylandı yap
            appointment.IsConfirmed = true;
            _context.Update(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // 5. RANDEVUYU SİL / REDDET - SADECE YÖNETİCİ
        // ==========================================
        [Authorize(Roles = "Admin")] // SADECE YÖNETİCİ SİLEBİLİR/REDDEDEBİLİR
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // ==========================================
        // 6. AJAX İÇİN MÜSAİT SAATLERİ HESAPLA (AYNI KALDI)
        // ==========================================
        [HttpGet]
        public JsonResult GetAvailableHours(int trainerId, DateTime date)
        {
            var trainer = _context.Trainers.Find(trainerId);
            if (trainer == null) return Json(new List<string>());

            var bookedTimes = _context.Appointments
                .Where(a => a.TrainerId == trainerId && a.AppointmentDate.Date == date.Date)
                .Select(a => a.AppointmentDate.ToString("HH:mm"))
                .ToList();

            List<string> availableSlots = new List<string>();

            TimeSpan current = trainer.WorkStart == TimeSpan.Zero ? new TimeSpan(9, 0, 0) : trainer.WorkStart;
            TimeSpan end = trainer.WorkEnd == TimeSpan.Zero ? new TimeSpan(18, 0, 0) : trainer.WorkEnd;

            while (current < end)
            {
                string timeString = current.ToString(@"hh\:mm");
                if (!bookedTimes.Contains(timeString))
                {
                    availableSlots.Add(timeString);
                }
                current = current.Add(TimeSpan.FromHours(1));
            }

            return Json(availableSlots);
        }

        // ==========================================
        // 7. KULLANICININ KENDİ RANDEVULARI (ÜYE)
        // Bu sayfa, API'yi çağırarak veriyi çeker.
        // ==========================================
        [Authorize] // SADECE GİRİŞ YAPAN KULLANICILARA AÇIK
        public IActionResult MyAppointments()
        {
            return View();
        }
    }
}
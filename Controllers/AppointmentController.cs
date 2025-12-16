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
            var appointments = await _context.Appointments
                .Include(a => a.Trainer)
                .Include(a => a.Service)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            ViewBag.Count = appointments.Count;

            return View(appointments);
        }

        // ==========================================
        // 2. RANDEVU ALMA SAYFASINI AÇ (GET) - GİRİŞ ŞARTI
        // ==========================================
        [Authorize] // GİRİŞ YAPAN HERKES RANDEVU ALABİLİR
        public IActionResult Create()
        {
            // Salonları Doldur
            ViewData["GymId"] = new SelectList(_context.Gyms, "Id", "Name");

            // Hizmet seçimi kaldırıldı
            // ViewData["ServiceId"] = ...

            // Eğitmenleri başlangıçta boş gönderiyoruz, Salon seçince dolacak
            ViewData["TrainerId"] = new SelectList(new List<SelectListItem>(), "Value", "Text");

            return View();
        }

        // ==========================================
        // 3. RANDEVUYU KAYDET (POST) - KULLANICI ID'SİNİ EKLEME
        // ==========================================
        [Authorize] // GİRİŞ YAPAN HERKES RANDEVU ALABİLİR
        [HttpPost]
        [ValidateAntiForgeryToken]
        // KULLANICI ID'si modelde olmadığı için Bind'e eklenmedi. Manuel ekleyeceğiz.
        public async Task<IActionResult> Create([Bind("Id,AppointmentDate,TrainerId")] Appointment appointment)
        {
            // Oturum açan kullanıcının ID'sini alıyoruz
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            appointment.UserId = userId; // KRİTİK

            // Veritabanı Constraint Hatasını Çözmek İçin Varsayılan Hizmet Atama
            // Kullanıcı arayüzünden seçim kaldırıldığı için, veritabanı 'Not Null' hatası veriyor.
            // Bu yüzden arka planda otomatik olarak ilk hizmeti atıyoruz.
            var defaultService = _context.Services.FirstOrDefault();
            if (defaultService != null)
            {
                appointment.ServiceId = defaultService.Id;
            }
            else
            {
                // Eğer hiç hizmet yoksa, null hatası almamak için geçici bir çözüm gerekebilir
                // Ancak sistemde en az bir hizmet olduğu varsayılıyor.
                ModelState.AddModelError("", "Sistemde kayıtlı hizmet bulunamadı. Lütfen yönetici ile iletişime geçin.");
                return View(appointment);
            }

            if (ModelState.IsValid)
            {
                // Varsayılan olarak IsConfirmed = false (Beklemede) kaydedilir.
                _context.Add(appointment);
                await _context.SaveChangesAsync();

                // Başarılı kayıttan sonra kullanıcının kendi randevu listesine yönlendir
                return RedirectToAction(nameof(MyAppointments));
            }

            // Hata olursa listeleri tekrar doldur
            // ViewData["ServiceId"] = ...

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
        // ==========================================
        // 4. RANDEVUYU ONAYLA (SADECE YÖNETİCİ)
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
        // YENİ: RANDEVU DÜZENLE (GET) - SADECE YÖNETİCİ
        // ==========================================
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            // Hizmetleri Doldur
            ViewData["ServiceId"] = new SelectList(_context.Services, "Id", "Name", appointment.ServiceId);

            // Eğitmenleri Doldur
            var trainers = _context.Trainers.Select(t => new
            {
                Id = t.Id,
                FullName = t.FirstName + " " + t.LastName
            }).ToList();
            ViewData["TrainerId"] = new SelectList(trainers, "Id", "FullName", appointment.TrainerId);

            return View(appointment);
        }

        // ==========================================
        // YENİ: RANDEVU DÜZENLE (POST) - SADECE YÖNETİCİ
        // ==========================================
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppointmentDate,TrainerId,ServiceId,UserId,IsConfirmed")] Appointment appointment)
        {
            if (id != appointment.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Appointments.Any(e => e.Id == appointment.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

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
        // YENİ: SALONA GÖRE EĞİTMENLERİ GETİR (AJAX)
        // ==========================================
        [HttpGet]
        public JsonResult GetTrainersByGym(int gymId)
        {
            var trainers = _context.Trainers
                .Where(t => t.GymId == gymId)
                .Select(t => new
                {
                    value = t.Id,
                    text = t.FirstName + " " + t.LastName + " (" + t.ExpertiseArea + ")"
                })
                .ToList();

            return Json(trainers);
        }

        // ==========================================
        // 7. KULLANICININ KENDİ RANDEVULARI (ÜYE)
        // Bu sayfa, API'yi çağırarak veriyi çeker.
        // ==========================================
        [Authorize] // SADECE GİRİŞ YAPAN KULLANICILARA AÇIK
        public async Task<IActionResult> MyAppointments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointments = await _context.Appointments
                .Include(a => a.Trainer)
                .ThenInclude(t => t.Gym)
                // .Include(a => a.Service) // Hizmet opsiyonel veya kaldırıldı
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }
    }
}
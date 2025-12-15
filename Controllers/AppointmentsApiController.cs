using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SporSalonuUygulamasi.Data;
using SporSalonuUygulamasi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

// API Controller'lar [ApiController] ve base sınıfı Controller değil ControllerBase olmalıdır.
[Route("api/[controller]")]
[ApiController]
// Bu API'ye sadece giriş yapanlar erişebilir.
[Authorize]
public class AppointmentsApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    // Constructor (Dependency Injection)
    public AppointmentsApiController(ApplicationDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: api/AppointmentsApi/MyAppointments
    // Mevcut kullanıcının tüm randevularını getirir (LINQ ile filtreleme).
    [HttpGet("MyAppointments")]
    public async Task<ActionResult<IEnumerable<Appointment>>> GetMyAppointments()
    {
        // 1. Oturum açan kullanıcının ID'sini alıyoruz
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
        {
            // Eğer kullanıcı ID'si bulunamazsa yetkisiz erişim hatası döndür
            return Unauthorized();
        }

        // 2. LINQ Sorgusu: Veritabanından sadece bu kullanıcıya ait randevuları çekiyoruz.
        var myAppointments = await _context.Appointments
            .Include(a => a.Service) // Hizmet bilgilerini de getir
            .Include(a => a.Trainer) // Eğitmen bilgilerini de getir
            .Where(a => a.UserId == userId) // SADECE Kullanıcı ID'si eşleşenleri filtrele!
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();

        if (myAppointments == null || myAppointments.Count == 0)
        {
            return NotFound("Bu kullanıcıya ait randevu bulunamadı.");
        }

        // 3. Randevuları JSON formatında döndür
        return Ok(myAppointments);
    }
}

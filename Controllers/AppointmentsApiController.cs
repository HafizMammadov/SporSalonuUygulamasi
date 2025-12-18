using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonuUygulamasi.Data;

namespace SporSalonuUygulamasi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // 1️⃣ TÜM RANDEVULAR (LINQ + DTO)
        // GET: api/AppointmentsApi/Appointments
        // ==========================================
        [HttpGet("Appointments")]
        public IActionResult GetAppointments()
        {
            var appointments = _context.Appointments
                .Include(a => a.Trainer)
                .OrderBy(a => a.AppointmentDate)
                .Select(a => new
                {
                    a.Id,
                    a.AppointmentDate,
                    a.IsConfirmed,
                    TrainerName = a.Trainer.FirstName + " " + a.Trainer.LastName
                })
                .ToList();

            return Ok(appointments);
        }

        // ==========================================
        // 2️⃣ TÜM ANTRENÖRLER
        // GET: api/AppointmentsApi/Trainers
        // ==========================================
        [HttpGet("Trainers")]
        public IActionResult GetTrainers()
        {
            var trainers = _context.Trainers
                .Select(t => new
                {
                    t.Id,
                    FullName = t.FirstName + " " + t.LastName
                })
                .ToList();

            return Ok(trainers);
        }

        // ==========================================
        // 3️⃣ BELİRLİ TARİHTE UYGUN ANTRENÖRLER
        // GET: api/AppointmentsApi/AvailableTrainers?date=2025-01-10
        // ==========================================
        [HttpGet("AvailableTrainers")]
        public IActionResult GetAvailableTrainers(DateTime date)
        {
            var availableTrainers = _context.Trainers
                .Where(t =>
                    !_context.Appointments.Any(a =>
                        a.TrainerId == t.Id &&
                        a.AppointmentDate.Date == date.Date
                    )
                )
                .Select(t => new
                {
                    t.Id,
                    FullName = t.FirstName + " " + t.LastName
                })
                .ToList();

            return Ok(availableTrainers);
        }

        // ==========================================
        // 4️⃣ TÜM ÜYELER (AppUser)
        // GET: api/AppointmentsApi/Users
        // ==========================================
        [HttpGet("Users")]
        public IActionResult GetUsers()
        {
            var users = _context.Users
                .Select(u => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email
                })
                .ToList();

            return Ok(users);
        }
    }
}

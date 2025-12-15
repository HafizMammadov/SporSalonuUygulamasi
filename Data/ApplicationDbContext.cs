using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SporSalonuUygulamasi.Models;

namespace SporSalonuUygulamasi.Data
{
    // IdentityDbContext kullanıyoruz çünkü giriş/çıkış işlemlerin var
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Gym> Gyms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Burada artık HourlyRate veya GymService ayarı YOK.
            // Sadece gerekirse özel ayarlar eklenir.
        }
    }
}
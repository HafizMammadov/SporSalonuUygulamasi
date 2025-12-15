using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SporSalonuUygulamasi.Models;

namespace SporSalonuUygulamasi.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Veritabanı tablolarımız (DbSet'leriniz)
        public DbSet<Gym> Gyms { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Appointment> Appointments { get; set; }


        // Model oluşturulurken ek yapılandırmaları yapmak için kullanılır.
        // ApplicationDbContext.cs dosyası içinde...


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Identity için temel model oluşturma işlemini çağırıyoruz (Zorunlu!)
            base.OnModelCreating(modelBuilder);

            // =========================================================
            // 🔥 ÇÖZÜM 1: Appointment - Trainer İlişkisini Netleştirme
            // Foreign Key belirsizliği ve Cascade Path sorununu çözer.
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Trainer)
                .WithMany(t => t.Appointments!) // Trainer.cs'deki Appointments koleksiyonunu belirtir
                .HasForeignKey(a => a.TrainerId) // Yabancı anahtarın TrainerId olduğunu netleştirir
                .OnDelete(DeleteBehavior.NoAction); // Multiple Cascade Paths Hatası Çözümü


            // =========================================================
            // 🔥 ÇÖZÜM 2: Trainer - Service İlişkisini Netleştirme (Sizin son hatanızın çözümü)
            // Foreign Key belirsizliği ve Cascade Path sorununu çözer.

            modelBuilder.Entity<Trainer>()
                .HasOne(t => t.Service) // Trainer'ın Service nesnesini kullanır
                .WithMany(s => s.Trainers!) // Service.cs'deki Trainers koleksiyonunu belirtir
                .HasForeignKey(t => t.ServiceId) // Yabancı anahtarın ServiceId olduğunu netleştirir
                .OnDelete(DeleteBehavior.NoAction); // Multiple Cascade Paths Hatası Çözümü


            // =========================================================
            // ⚙️ ÇÖZÜM 3: DECIMAL HASSASİYET UYARISINI GİDERME
            // Service.Price alanının veritabanında doğru tanımlanmasını sağlar.
            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasColumnType("decimal(18, 2)");
        }
    }
}
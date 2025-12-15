using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporSalonuUygulamasi.Models
{
    public class Trainer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        public string LastName { get; set; }

        public string ExpertiseArea { get; set; }

        public TimeSpan WorkStart { get; set; } = new TimeSpan(9, 0, 0);
        public TimeSpan WorkEnd { get; set; } = new TimeSpan(17, 0, 0);

        // Salon Bağlantısı (Foreign Key)
        [Display(Name = "Çalıştığı Salon")]
        public int? GymId { get; set; }

        [ForeignKey("GymId")]
        public Gym Gym { get; set; }
    }
}
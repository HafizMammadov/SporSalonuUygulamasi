using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // ForeignKey için şart

namespace SporSalonuUygulamasi.Models
{
    public class Trainer
    {
        [Key]
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Ad Soyad girilmesi zorunludur.")]
        public string FullName { get; set; } = null!;

        public string? ExpertiseArea { get; set; } // Uzmanlık (Fitness vb.)
        public string? PhotoUrl { get; set; }

        // --- 1. KURAL: EĞİTMEN BİR SALONA AİT OLMALI ---
        [Required(ErrorMessage = "Lütfen eğitmenin çalışacağı salonu seçiniz.")]
        public int GymId { get; set; } // Hangi salonda?

        [ForeignKey("GymId")]
        public virtual Gym? Gym { get; set; }
        // -----------------------------------------------

        // --- 2. KURAL: EĞİTMEN BİR HİZMET VERMELİ ---
        [Required(ErrorMessage = "Lütfen eğitmenin uzmanlık alanını (hizmeti) seçiniz.")]
        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service? Service { get; set; }
        // --------------------------------------------

        public virtual ICollection<Appointment>? Appointments { get; set; }
    }
}
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SporSalonuUygulamasi.Models
{
    public class Gym
    {
        [Key]
        public int GymId { get; set; }

        [Required(ErrorMessage = "Salon adı zorunludur.")]
        public string Name { get; set; } = null!;

        // --- GÜNCELLEME: Adres artık zorunlu ---
        [Required(ErrorMessage = "Adres alanı boş bırakılamaz.")]
        public string Address { get; set; } = null!;

        public string? WorkingHours { get; set; }

        // İlişkiler
        public virtual ICollection<Trainer>? Trainers { get; set; }
    }
}
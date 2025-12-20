using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SporSalonuUygulamasi.Models
{
    public class Gym
    {
        [Key]
        public int Id { get; set; } 

        [Required(ErrorMessage = "Salon adı zorunludur.")]
        [Display(Name = "Salon Adı")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Adres zorunludur.")]
        [Display(Name = "Adres")]
        public string Address { get; set; }

        [Display(Name = "Çalışma Saatleri")]
        public string WorkingHours { get; set; } // Örn: "09:00-22:00"

        // Eğitmenlerle bağlantı
        public virtual ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();

       
    }
}
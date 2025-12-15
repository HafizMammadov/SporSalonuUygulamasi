using System.ComponentModel.DataAnnotations;

namespace SporSalonuUygulamasi.Models
{
    public class Gym
    {
        [Key]
        public int GymId { get; set; }

        [Required(ErrorMessage = "Salon adı zorunludur.")]
        [Display(Name = "Salon Adı")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Adres zorunludur.")]
        [Display(Name = "Adres")]
        public string Address { get; set; }

       // [Required(ErrorMessage = "Çalışma saatleri zorunludur.")]
        [Display(Name = "Çalışma Saatleri")]
        public string WorkingHours { get; set; } // Örn: "09:00-22:00"

        public virtual ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();
        public virtual ICollection<GymService> GymServices { get; set; } = new List<GymService>();
    }
}
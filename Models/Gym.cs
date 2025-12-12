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

        public string? Address { get; set; }
        public string? WorkingHours { get; set; }

        public virtual ICollection<Trainer>? Trainers { get; set; }
    }
}
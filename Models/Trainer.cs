using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SporSalonuUygulamasi.Models
{
    public class Trainer
    {
        [Key]
        public int TrainerId { get; set; }

        [Required]
        public string FullName { get; set; } = null!; // Bu alan kesin dolu olmalı dedik

        public string? ExpertiseArea { get; set; }
        public string? PhotoUrl { get; set; }

        // İlişkiler
        public int GymId { get; set; }
        public virtual Gym? Gym { get; set; } // ? Eklendi

        public int ServiceId { get; set; }
        public virtual Service? Service { get; set; } // ? Eklendi

        public virtual ICollection<Appointment>? Appointments { get; set; } // ? Eklendi
    }
}
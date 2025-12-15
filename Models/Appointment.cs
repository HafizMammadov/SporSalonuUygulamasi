using System;
using System.ComponentModel.DataAnnotations;

namespace SporSalonuUygulamasi.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public bool IsConfirmed { get; set; } = false;

        // İlişkiler
        public string AppUserId { get; set; } // ID null olabilir veya boş gelebilir
        public virtual AppUser AppUser { get; set; } // SORU İŞARETİ EKLENDİ

        public int TrainerId { get; set; }
        public virtual Trainer Trainer { get; set; } // SORU İŞARETİ EKLENDİ

        public int ServiceId { get; set; }
        public virtual Service Service { get; set; } // SORU İŞARETİ EKLENDİ
    }
}
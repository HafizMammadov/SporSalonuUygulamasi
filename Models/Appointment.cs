using System.ComponentModel.DataAnnotations.Schema;

namespace SporSalonuUygulamasi.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }

        // İlişkisel Özellikler (Foreign Keys)

        // 1. Kullanıcı ID'si (Eksik Olan Kısım!)
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser User { get; set; } // Hangi kullanıcı bu randevuyu aldı?

        // 2. Eğitmen ID'si
        public int TrainerId { get; set; }
        [ForeignKey("TrainerId")]
        public Trainer Trainer { get; set; }

        // 3. Hizmet ID'si (Opsiyonel yapıldı)
        public int? ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

        public bool IsConfirmed { get; set; } = false; // Randevu Onay Durumu
    }
}
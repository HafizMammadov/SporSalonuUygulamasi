using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporSalonuUygulamasi.Models
{
    public class GymService
    {
        public int Id { get; set; }

        public int GymId { get; set; }
        [ForeignKey("GymId")]
        public Gym Gym { get; set; }

        public int ServiceId { get; set; }
        [ForeignKey("ServiceId")]
        public Service Service { get; set; }

        // BU EKSİKTİ, BUNU EKLİYORUZ:
        [Display(Name = "Saatlik Ücret")]
        public decimal HourlyRate { get; set; }
    }
}
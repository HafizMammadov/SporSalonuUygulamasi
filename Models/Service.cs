using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // ForeignKey için gerekli

namespace SporSalonuUygulamasi.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        public string Name { get; set; } = null!; // Örn: Pilates, Yoga

        [Required(ErrorMessage = "Süre girmek zorunludur.")]
        public int DurationMinutes { get; set; } // Süre (dk)

        [Required(ErrorMessage = "Ücret girmek zorunludur.")]
        public decimal Price { get; set; } // Ücret

        public string? Description { get; set; }

        
        [Required(ErrorMessage = "Lütfen bir salon seçiniz.")]
        public int GymId { get; set; }

        [ForeignKey("GymId")]
        public virtual Gym? Gym { get; set; }
     
    }
}
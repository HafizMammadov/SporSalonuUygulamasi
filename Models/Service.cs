using System.ComponentModel.DataAnnotations;

namespace SporSalonuUygulamasi.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Hizmet adı gereklidir.")]
        public string Name { get; set; } = null!; // Zorunlu alan

        public int DurationMinutes { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; } // ? Eklendi
        public string? ImageUrl { get; set; } // ? Eklendi

        public virtual ICollection<Trainer>? Trainers { get; set; }
    }
}
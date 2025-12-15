using System.ComponentModel.DataAnnotations;

namespace SporSalonuUygulamasi.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } // Hizmet Adı (Örn: Pilates, Boks)

        public int DurationMinutes { get; set; } = 60; // Süre (Dakika)

        public decimal Price { get; set; } // Ücret
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporSalonuUygulamasi.Models
{
    public class Trainer
    {
        [Key]
        public int TrainerId { get; set; }

        [Required(ErrorMessage = "Ad zorunludur.")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur.")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [NotMapped]
        [Display(Name = "Ad Soyad")]
        public string FullName => $"{FirstName} {LastName}";

        [Required(ErrorMessage = "Uzmanlık alanı zorunludur.")]
        [Display(Name = "Uzmanlık Alanı")]
        public string ExpertiseArea { get; set; }

        [Required(ErrorMessage = "Saatlik ücret zorunludur.")]
        [Display(Name = "Saatlik Ücret (₺)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Saatlik ücret 0'dan büyük olmalıdır.")]
        public decimal HourlyRate { get; set; }

       // [Required(ErrorMessage = "Çalışma saatleri zorunludur.")]
        [Display(Name = "Çalışma Saatleri")]
        public string WorkingHours { get; set; }

        [Display(Name = "Müsaitlik Durumu")]
        public bool IsAvailable { get; set; } = true;

        public string PhotoUrl { get; set; }

        // --- İLİŞKİLER ---

        [Required(ErrorMessage = "Spor salonu seçimi zorunludur.")]
        [Display(Name = "Spor Salonu")]
        public int GymId { get; set; }

        [ForeignKey("GymId")]
        virtual public  Gym Gym { get; set; }
    }
}
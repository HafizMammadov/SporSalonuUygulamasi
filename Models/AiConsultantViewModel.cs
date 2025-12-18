using System.ComponentModel.DataAnnotations;

namespace SporSalonuUygulamasi.Models
{
    public class AiConsultantViewModel
    {
        [Required(ErrorMessage = "Yaş alanı zorunludur.")]
        [Range(10, 100, ErrorMessage = "Geçerli bir yaş giriniz (10-100).")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Boy bilgisi zorunludur.")]
        [Range(100, 250, ErrorMessage = "Geçerli bir boy giriniz (cm).")]
        public int Height { get; set; }

        [Required(ErrorMessage = "Kilo bilgisi zorunludur.")]
        [Range(30, 200, ErrorMessage = "Geçerli bir kilo giriniz (kg).")]
        public int Weight { get; set; }

        [Required(ErrorMessage = "Cinsiyet seçimi zorunludur.")]
        public string Gender { get; set; } // "Erkek", "Kadın"

        [Required(ErrorMessage = "Hedef seçimi zorunludur.")]
        public string Goal { get; set; } // "Kilo Vermek", "Kas Yapmak", "Formu Korumak"

        [Required(ErrorMessage = "Aktivite seviyesi seçimi zorunludur.")]
        public string ActivityLevel { get; set; } // "Hareketsiz", "Az Hareketli", "Orta", "Çok Hareketli"
        
        // Optional file upload for future use or descriptive prompt
        public string AdditionalNotes { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporSalonuUygulamasi.Models
{
    public class GymService
    {
        [Key]
        public int Id { get; set; }

        public int GymId { get; set; }

        [ForeignKey("GymId")]
        public virtual Gym Gym { get; set; }

        [Required(ErrorMessage = "Hizmet adı zorunludur.")]
        [Display(Name = "Hizmet Adı")]
        public string ServiceName { get; set; }

        [Required(ErrorMessage = "Saatlik ücret zorunludur.")]
        [Display(Name = "Saatlik Ücret (₺)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Saatlik ücret 0'dan büyük olmalıdır.")]
        public decimal HourlyRate { get; set; }
    }
}

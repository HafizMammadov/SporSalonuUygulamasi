using System.ComponentModel.DataAnnotations;

namespace SporSalonuUygulamasi.Models
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Bu bilgi zorunludur.")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Bu bilgi zorunludur.")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Display(Name = "E-posta")]
        public string Email { get; set; } // Read-only typically

        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi")]
        public DateTime? BirthDate { get; set; }
    }
}

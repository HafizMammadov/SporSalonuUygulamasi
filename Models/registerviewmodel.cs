using System.ComponentModel.DataAnnotations;

namespace SporSalonuUygulamasi.Models
{
    // Yeni üye (kayıt) bilgileri için model
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Bu bilgi zorunludur.")]
        [Display(Name = "Ad")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Bu bilgi zorunludur.")]
        [Display(Name = "Soyad")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "Bu bilgi zorunludur.")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir E-posta adresi girin.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bu bilgi zorunludur.")]
        // Şifrenin minimum uzunluk kısıtlaması (Ödev gereksinimlerine göre en az 6-8 karakter önerilir)
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunlugunda olmalidir.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifreyi Onayla")]
        // Compare niteliği ile şifrelerin eşleştiği kontrol edilir.
        [Compare("Password", ErrorMessage = "Şifre ve onay şifresi eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}

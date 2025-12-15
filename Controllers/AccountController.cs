using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SporSalonuUygulamasi.Models; // Modellerin olduğu yer

namespace SporSalonuUygulamasi.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ==========================================
        // LOGIN (GİRİŞ) İŞLEMLERİ
        // ==========================================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Giriş yapmayı dene
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    // ---> İŞTE BURASI: Giriş Başarılıysa Direkt Ana Sayfaya Git <---
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Giriş başarısız. Email veya şifre hatalı.");
            }
            return View(model);
        }

        // ==========================================
        // REGISTER (KAYIT) İŞLEMLERİ
        // ==========================================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Yeni kullanıcı nesnesi oluştur
                var user = new AppUser
                {
                    UserName = model.Email, // Kullanıcı adı email ile aynı olsun
                    Email = model.Email,
                    FirstName = model.Ad,
                    LastName = model.Soyad
                };

                // Kullanıcıyı veritabanına kaydet
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Kayıt olur olmaz otomatik giriş yaptır
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // ---> İŞTE BURASI: Kayıt Başarılıysa Direkt Ana Sayfaya Git <---
                    return RedirectToAction("Index", "Home");
                }

                // Hata varsa (örn: şifre çok basitse) göster
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // ==========================================
        // LOGOUT (ÇIKIŞ) İŞLEMİ
        // ==========================================
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
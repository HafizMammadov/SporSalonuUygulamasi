using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SporSalonuUygulamasi.Models; // ViewModel'ler ve AppUser için
using System.Threading.Tasks;

// AppUser'ın tam adını kullanmak için projenizin kök adını buraya eklemelisiniz.
namespace SporSalonuUygulamasi.Controllers
{
    public class AccountController : Controller
    {
        // AppUser kullandığımız için Identity servislerini ona göre ayarlıyoruz.
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: /Account/LoginRegister (Anasayfa olarak bu sayfayı kullanacağız)
        [HttpGet]
        public IActionResult LoginRegister(string returnUrl = null)
        {
            // Kullanıcı zaten giriş yapmışsa ana panele yönlendirilir.
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Dashboard", "Home");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Yeni Üye Kaydı (Register)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // AppUser nesnesi oluşturulur.
                var user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.Ad, // AppUser'da Ad ve Soyad alanları varsa (varsayıyoruz)
                    LastName = model.Soyad
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Ödev Gereksinimi: Kullanıcıya 'Uye' rolünü atama.
                    const string UYE_ROLE = "Uye";
                    if (!await _roleManager.RoleExistsAsync(UYE_ROLE))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(UYE_ROLE));
                    }
                    await _userManager.AddToRoleAsync(user, UYE_ROLE);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Dashboard", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View("LoginRegister");
        }

        // POST: Giriş Yap (Login)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Dashboard", "Home");
                }

                ModelState.AddModelError(string.Empty, "Geçersiz e-posta veya şifre.");
            }
            return View("LoginRegister");
        }

        // POST: Çıkış (Logout) işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

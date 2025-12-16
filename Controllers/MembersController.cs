using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporSalonuUygulamasi.Models;
using SporSalonuUygulamasi.Utility;
using System.Linq;
using System.Threading.Tasks;

namespace SporSalonuUygulamasi.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class MembersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public MembersController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        // 1. LİSTELEME
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            ViewBag.Count = users.Count;
            return View(users);
        }

        // 2. DÜZENLEME (GET)
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }

        // 3. DÜZENLEME (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, AppUser appUser)
        {
            if (id != appUser.Id) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (ModelState.IsValid)
            {
                user.FirstName = appUser.FirstName;
                user.LastName = appUser.LastName;
                user.Email = appUser.Email;
                user.UserName = appUser.Email; // UserName genelde Email ile aynı tutulur

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(user);
        }

        // 4. SİLME (POST) - Direkt Silme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                try
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View("Index", _userManager.Users.ToList()); // Hata varsa listeye dön ama hatayı göster (zor çünkü redirect yapıyoruz genelde)
                    }
                }
                catch (Exception ex)
                {
                     // FK hatası vs.
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

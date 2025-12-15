using Microsoft.AspNetCore.Identity;
using SporSalonuUygulamasi.Models;
using SporSalonuUygulamasi.Utility; // Roles sınıfına erişim için

namespace SporSalonuUygulamasi.Data
{
    public class DbInitializer
    {
        // Gerekli servisleri (RoleManager, UserManager) DI ile alıyoruz
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public DbInitializer(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task Initialize()
        {
            // 1. Rolleri Oluşturma (Eğer yoksa)
            // Eğer Admin rolü yoksa oluştur
            if (await _roleManager.FindByNameAsync(Roles.Admin) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }
            // Eğer User rolü yoksa oluştur
            if (await _roleManager.FindByNameAsync(Roles.User) == null)
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.User));
            }

            // 2. İlk Admin Kullanıcısını Oluşturma (Eğer yoksa)
            if (await _userManager.FindByEmailAsync("admin@spor.com") == null)
            {
                var adminUser = new AppUser
                {
                    UserName = "admin@spor.com",
                    Email = "admin@spor.com",
                    FirstName = "Yönetici",
                    LastName = "Admin",
                    EmailConfirmed = true
                };

                // Kullanıcıyı oluştur ve şifre ata
                // **ÖNEMLİ: Şifrenizi daha karmaşık bir şeyle değiştirin!**
                await _userManager.CreateAsync(adminUser, "Admin123*");

                // Kullanıcıya Admin rolünü ata
                await _userManager.AddToRoleAsync(adminUser, Roles.Admin);
            }
        }
    }
}

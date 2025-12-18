using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SporSalonuUygulamasi.Data;
using SporSalonuUygulamasi.Models;
using SporSalonuUygulamasi.Utility; // EKLEND�: Roles s�n�f�na eri�im i�in

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabani Baglantisi
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Identity (Uyelik) Ayarlari
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Cookie ayarlari - giris yapilmadan erisilemeyecek sayfalarda Login sayfasina yonlendir
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Hata Ayiklama Modu
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// VARSAYILAN ROUTE: Uygulama Login sayfasindan baslasin
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// ==========================================
// 5. ROL OLUSTURMA ve ILK ADMIN TANIMLAMA (G�NCELLENM�� VE TEM�ZLENM�� BLOK)
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    // Rol adlar�n� Utility s�n�f�ndan al�yoruz. "Uye" yerine "User" kullan�ld�.
    string[] roleNames = { Roles.Admin, Roles.User };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Admin kullan�c� bilgileri
    string adminEmail = "admin@sakarya.edu.tr";
    string adminPassword = "Haf�z1234"; // L�tfen bu �ifreyi daha g�venli bir �eyle de�i�tirin!
    string adminRole = Roles.Admin; // Utility s�n�f�ndaki sabit kullan�ld�.

    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new AppUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FirstName = "Proje",
            LastName = "Admin"
        };
        var result = await userManager.CreateAsync(adminUser, adminPassword);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
}
app.Run();
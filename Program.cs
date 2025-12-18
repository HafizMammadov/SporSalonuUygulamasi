using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SporSalonuUygulamasi.Data;
using SporSalonuUygulamasi.Models;
using SporSalonuUygulamasi.Utility;
using SporSalonuUygulamasi.Services;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. VERİTABANI (DATABASE)
// ==========================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// ==========================================
// 2. IDENTITY (KULLANICI YÖNETİMİ)
// ==========================================
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    // Şifre kuralları (Öğrenci projesi olduğu için esnek bıraktık)
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3; // Test kolaylığı için kısalttım
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
});

// ==========================================
// 3. MVC VE API AYARLARI
// ==========================================
builder.Services.AddControllersWithViews();

// Swagger (API testi için)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==========================================
// 4. ÖZEL SERVİSLER (YAPAY ZEKA)
// ==========================================
// Gemini (Metin Üretimi) için HttpClient servisi
builder.Services.AddHttpClient<GeminiAiService>();

// Resim URL Üretimi için Basit Servis (Yeni Eklenen)
builder.Services.AddScoped<SimpleImageService>();


var app = builder.Build();

// ==========================================
// 5. MIDDLEWARE (UYGULAMA AKIŞI)
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Kimlik doğrulama
app.UseAuthorization();  // Yetkilendirme

// ==========================================
// 6. ROUTE AYARLARI
// ==========================================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapControllers();

// ==========================================
// 7. ROL VE ADMİN OLUŞTURMA (SEED DATA)
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    // Rolleri oluştur
    string[] roleNames = { Roles.Admin, Roles.User };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Admin kullanıcısı oluştur
    string adminEmail = "admin@sakarya.edu.tr";
    string adminPassword = "Hafiz1234";
    string adminRole = Roles.Admin;

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
using Microsoft.AspNetCore.Mvc;
using SporSalonuUygulamasi.Models;
using SporSalonuUygulamasi.Services;

namespace SporSalonuUygulamasi.Controllers
{
    public class AiController : Controller
    {
        // Servisleri tanımlıyoruz
        private readonly GeminiAiService _geminiService;
        private readonly SimpleImageService _imageService;

        // Constructor ile servisleri içeri alıyoruz
        public AiController(GeminiAiService geminiService, SimpleImageService imageService)
        {
            _geminiService = geminiService;
            _imageService = imageService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new AiConsultantViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePlan(AiConsultantViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            // 1. ADIM: Prompt Hazırlama
            string prompt = $@"
Bana aşağıdaki özelliklere sahip bir kişi için kapsamlı bir diyet ve egzersiz programı hazırla.
Kişisel Bilgiler:
- Yaş: {model.Age}, Boy: {model.Height} cm, Kilo: {model.Weight} kg
- Cinsiyet: {model.Gender}, Hedef: {model.Goal}
- Aktivite Seviyesi: {model.ActivityLevel}
- Ek Notlar: {model.AdditionalNotes}

Lütfen yanıtı şu formatta ver (Markdown kullanarak):
# Kişiye Özel Sağlık Planı
## 1. Diyet Programı
(Günlük örnek menü ve beslenme tavsiyeleri)
## 2. Egzersiz Programı
(Haftalık plan ve hareketler)
";

            // 2. ADIM: Metni Getir (Hata olsa bile yedek metin gelir)
            string textResult = await _geminiService.GenerateDietAndWorkoutPlanAsync(prompt);

            // 3. ADIM: Resmi Getir (SimpleImageService kullanır)
            string imageUrl = _imageService.ResimUrlOlustur(model.Gender, model.Goal);

            // 4. ADIM: Sonuçları View'a Gönder
            ViewBag.PlanResult = textResult;
            ViewBag.GeneratedImageUrl = imageUrl;

            return View("Result");
        }
    }
}
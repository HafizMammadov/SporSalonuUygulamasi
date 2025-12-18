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

Lütfen yanıtı SADECE geçerli bir JSON formatında ver. Başka hiçbir metin veya açıklama ekleme.
JSON formatı şu şekilde olmalıdır:
{{
  ""diet_plan"": ""Markdown formatında detaylı diyet programı buraya..."",
  ""exercise_plan"": ""Markdown formatında detaylı egzersiz programı buraya...""
}}
";

            // 2. ADIM: Metni Getir
            string textResult = await _geminiService.GenerateDietAndWorkoutPlanAsync(prompt);

            // 3. ADIM: JSON Parse İşlemi
            string dietPlan = "";
            string exercisePlan = "";

            try
            {
                // Markdown kod bloklarını temizle (eğer gelirse)
                textResult = textResult.Replace("```json", "").Replace("```", "").Trim();

                using (var doc = System.Text.Json.JsonDocument.Parse(textResult))
                {
                    var root = doc.RootElement;
                    if (root.TryGetProperty("diet_plan", out var dietProp))
                    {
                        dietPlan = dietProp.GetString();
                    }
                    if (root.TryGetProperty("exercise_plan", out var exerciseProp))
                    {
                        exercisePlan = exerciseProp.GetString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Fallback: JSON parse edilemezse ham metni diyet kısmına bas, egzersizi boş geç veya hata mesajı ver
                dietPlan = textResult; 
                exercisePlan = "Ayrıştırma yapılamadı. Tüm plan Soldaki 'Beslenme Programı' sekmesindedir.";
            }

            // 4. ADIM: Resmi Getir (SimpleImageService kullanır)
            string imageUrl = _imageService.ResimUrlOlustur(model.Gender, model.Goal);

            // 5. ADIM: Sonuçları View'a Gönder
            ViewBag.DietPlan = dietPlan;
            ViewBag.ExercisePlan = exercisePlan;
            ViewBag.GeneratedImageUrl = imageUrl;

            return View("Result");
        }
    }
}
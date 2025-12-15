using Microsoft.AspNetCore.Mvc;
using SporSalonuUygulamasi.Models;
using System.Threading.Tasks;

// Not: Artık Newtonsoft.Json ve System.Text'e ihtiyacımız yok
// Bu kütüphaneler gerçek API bağlantısı için gerekliydi.

namespace SporSalonuUygulamasi.Controllers
{
    public class AiController : Controller
    {
        // GET: Sayfayı Aç
        public IActionResult Index()
        {
            return View(new AiViewModel());
        }

        // POST: Form gönderilince Simülasyon çalışır (B PLAN)
        [HttpPost]
        public async Task<IActionResult> Index(AiViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // GÜVENLİ VE HATASIZ ÇALIŞAN SİMÜLASYON MODU

            // 2 saniye bekle (Yapay zeka düşünüyormuş gibi hissettirir)
            await Task.Delay(2000);

            // Kullanıcının girdiği bilgilere göre dinamik görünen sahte bir cevap oluşturuyoruz.
            string sahteCevap = $@"
                <h4>Merhaba! Ben AI Antrenörün.</h4>
                <p>Girdiğin bilgilere göre (<b>{model.Height} cm</b> boy, <b>{model.Weight} kg</b> ağırlık ve hedef: <b>{model.Goal}</b>) senin için özel plan hazırladım.</p>
                
                <hr>
                
                <h5>🥗 Günlük Beslenme Önerisi</h5>
                <ul>
                    <li><b>Kahvaltı:</b> 2 adet haşlanmış yumurta, bir kase yulaf ezmesi ve bir adet meyve.</li>
                    <li><b>Öğle:</b> Yağsız ızgara et/tavuk (150g) ve büyük bir yeşil salata.</li>
                    <li><b>Ara Öğün:</b> Bir avuç ceviz/badem veya protein barı.</li>
                    <li><b>Akşam:</b> Hafif zeytinyağlı sebze yemeği ve bir kase yoğurt.</li>
                </ul>

                <h5>💪 Antrenman Programı (Hedef: {model.Goal})</h5>
                <p>Haftada 3 gün bu temel hareketleri yapmalısın:</p>
                <ul>
                    <li><b>1. Hareket:</b> Squat (3 set x 12 tekrar) - Ana Bacak hareketi.</li>
                    <li><b>2. Hareket:</b> Bench Press (3 set x 10 tekrar) - Göğüs kasları için.</li>
                    <li><b>3. Hareket:</b> Barbell Row (3 set x 10 tekrar) - Sırt kasları için.</li>
                </ul>
                
                <br>
                <div class='alert alert-info'>
                    <b>AI Notu:</b> Bu program başlangıç seviyesi içindir ve {model.Goal} hedefine yönelik ayarlanmıştır.
                </div>";

            model.AiResponse = sahteCevap;

            return View(model);
        }
    }
}
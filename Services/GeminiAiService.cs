using System.Net;

namespace SporSalonuUygulamasi.Services
{
    public class GeminiAiService
    {
        private readonly HttpClient _httpClient;

        // Constructor yapısını bozmuyoruz ki Program.cs hata vermesin.
        public GeminiAiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            // Cevap uzun sürerse kesilmesin diye süreyi uzatıyoruz
            _httpClient.Timeout = TimeSpan.FromSeconds(60);
        }

        public async Task<string> GenerateDietAndWorkoutPlanAsync(string prompt)
        {
            // 1. Prompt Ayarı: Yapay zekaya Türkçe ve Markdown konuşması için ekleme yapıyoruz.
            string finalPrompt = prompt + " . Lütfen cevabı Türkçe dilinde, profesyonel bir spor hocası gibi, şık bir Markdown formatında ver. Başlıkları # ile belirt. Sadece diyet ve sporu yaz, gereksiz sohbet etme.";

            // 2. URL Encoding: Yazıyı link formatına çeviriyoruz.
            string encodedPrompt = WebUtility.UrlEncode(finalPrompt);

            // 3. YENİ SERVİS: Pollinations AI (Text). API Key GEREKTİRMEZ.
            // Google API modelleriyle uğraşmadan direkt cevabı verir.
            string url = $"https://text.pollinations.ai/{encodedPrompt}?model=openai";

            try
            {
                // GET isteği gönderiyoruz
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return "Servis şu an yanıt veremiyor, lütfen butona tekrar basın.";
                }

                // Gelen cevap direkt metindir, JSON parse etmeye gerek yok.
                string resultText = await response.Content.ReadAsStringAsync();

                return resultText;
            }
            catch
            {
                return "Bağlantı hatası oluştu. Lütfen internetinizi kontrol edip tekrar deneyin.";
            }
        }
    }
}
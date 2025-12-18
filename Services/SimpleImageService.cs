namespace SporSalonuUygulamasi.Services
{
    public class SimpleImageService
    {
        // Bu metot, ücretsiz bir AI resim oluşturucu servisine giden linki hazırlar.
        // API Key gerektirmez, kota sorunu yoktur. Sunumda hayat kurtarır.
        public string ResimUrlOlustur(string cinsiyet, string hedef)
        {
            // İngilizceye çevirelim ki AI daha iyi anlasın
            string genderEn = (cinsiyet.ToLower() == "erkek" || cinsiyet.ToLower() == "male") ? "male" : "female";
            string goalEn = hedef.Contains("Kas") ? "muscle building fitness model" : "slim fit athletic body";

            // Dinamik Prompt (Komut)
            string prompt = $"realistic photo of a {genderEn} with {goalEn} body type, in a modern gym, cinematic lighting, 8k resolution, full body shot, sportswear";

            // URL Encoding (Boşlukları %20 yapmak için)
            string encodedPrompt = System.Net.WebUtility.UrlEncode(prompt);

            // Pollinations.ai ücretsiz ve key istemeyen bir servistir.
            return $"https://image.pollinations.ai/prompt/{encodedPrompt}?width=1024&height=1024&nologo=true";
        }
    }
}
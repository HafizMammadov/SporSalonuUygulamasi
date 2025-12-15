namespace SporSalonuUygulamasi.Models
{
    public class AiViewModel
    {
        public int Age { get; set; }        // Yaş
        public int Height { get; set; }     // Boy
        public int Weight { get; set; }     // Kilo
        public string Gender { get; set; }  // Cinsiyet
        public string Goal { get; set; }    // Hedef (Kilo ver / Kas yap)
        public string? AiResponse { get; set; } // Yapay zekadan gelen cevap
    }
}

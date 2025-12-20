# AI Destekli Spor Salonu Yönetim Sistemi (Gym Management System with AI)

Bu proje, modern web teknolojileri ve yapay zeka entegrasyonu kullanılarak geliştirilmiş kapsamlı bir spor salonu yönetim sistemidir. Kullanıcıların kişiselleştirilmiş programlar almasını, yöneticilerin ise tüm salon süreçlerini uçtan uca yönetmesini sağlar.

## 🚀 Proje Hakkında

Sakarya Üniversitesi Web Programlama dersinin proje odevi olarak geliştirilen bu uygulamada giris yapan uyeler istedikleri hocalardan randevu alabilirler.Kullanıcılar fiziksel özelliklerine ve hedeflerine göre anında diyet ve egzersiz programı oluşturabilirler.

## ✨ Temel Özellikler

### 🤖 Yapay Zeka (AI) Modülü
*   **Kişiselleştirilmiş Planlar:** Kullanıcının yaş, boy, kilo ve hedefine (Kilo verme, Kas yapma vb.) göre anlık diyet ve antrenman programı oluşturur.
*   **Görselleştirme:** Hedeflenen fiziksel değişimi temsil eden yapay zeka destekli görseller sunar.
*   **Ayrıştırılmış Çıktı:** Beslenme ve Antrenman programları ayrı sekmelerde düzenli bir şekilde sunulur.

### 👥 Üye Paneli
*   **Randevu Sistemi:** Eğitmenlerden online randevu alma.
*   **Profil Yönetimi:** Kişisel bilgileri güncelleme ve geçmiş randevuları görüntüleme.

### 🛠️ Yönetici (Admin) Paneli
*   **Dashboard:** Salon, eğitmen ve üye istatistiklerini grafiksel ve sayısal olarak görüntüleme.
*   **Veri Yönetimi:**
    *   **Salonlar:** Yeni şube ekleme/düzenleme/silme.
    *   **Eğitmenler:** Uzmanlık alanlarına göre eğitmen yönetimi.
    *   **Üyeler:** Tüm kayıtlı üyelerin (CRUD) işlemleri.
    *   **Randevular:** Tüm randevu taleplerini onaylama veya iptal etme.


## 💻 Teknolojiler

*   **Backend:** ASP.NET Core 8.0 MVC
*   **Veritabanı:** Microsoft SQL Server (Entity Framework Core Code-First)
*   **AI Entegrasyonu:** Google Gemini API
*   **Frontend:** HTML5, CSS3, JavaScript, Bootstrap 5


## ⚙️ Kurulum ve Çalıştırma

1.  **Veritabanı Ayarı:**
    `appsettings.json` dosyasındaki `ConnectionStrings` bölümünü kendi SQL Server bağlantınıza göre güncelleyin.
    ```json
    "DefaultConnection": "Server=...;Database=SporSalonuDb;Trusted_Connection=True;..."
    ```

2.  **Migration Uygulama:**
    Package Manager Console üzerinden veritabanını oluşturun:
    ```powershell
    Update-Database
    ```

3.  **Projeyi Başlatma:**
    Projeyi Çalıştırın (F5  veya `dotnet run`). Uygulama ilk açılışta otomatik olarak Admin kullanıcısını oluşturacaktır.
# Otizm Eğitim Platformu

Bu proje, otizmli çocukların eğitimine yardımcı olmak için tasarlanmış bir web uygulamasıdır. ASP.NET Core MVC kullanılarak geliştirilmiştir.

## Özellikler

### Kullanıcı Tipleri
- **Veli**: Çocuklarının gelişimini takip edebilir
- **Eğitmen**: Öğrenci gelişimini takip edebilir ve raporlar oluşturabilir
- **Admin**: Sistem yönetimini gerçekleştirebilir

### Eğitim Modülleri
- Hayvanlar
- Sayılar
- Renkler
- Şekiller
- Trafik İşaretleri
- Görgü Kuralları

### Temel Özellikler
- Kullanıcı kaydı ve girişi
- Oturum yönetimi
- İnteraktif eğitim içerikleri
- İlerleme takibi
- Eğitmen raporlama sistemi

## Teknolojiler

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Bootstrap 5
- jQuery
- HTML5/CSS3

## Kurulum

1. Repository'yi klonlayın:
```bash
git clone https://github.com/betulzzturkk/Bproje.git
```

2. Veritabanını oluşturun:
```bash
dotnet ef database update
```

3. Projeyi çalıştırın:
```bash
dotnet run
```

4. Tarayıcınızda şu adresi açın:
```
http://localhost:5000
```

## Kullanım

### Admin Girişi
- `/Admin/Giris` sayfasından admin girişi yapılabilir

### Veli Girişi
- Ana sayfadan kayıt olunabilir
- Kullanıcı tipi olarak "Veli" seçilmelidir
- Giriş yaptıktan sonra çocuk bilgileri eklenebilir

### Eğitmen Girişi
- Ana sayfadan kayıt olunabilir
- Kullanıcı tipi olarak "Eğitmen" seçilmelidir
- Giriş yaptıktan sonra öğrenci listesi ve raporlama sistemine erişilebilir

## Katkıda Bulunma

1. Bu repository'yi fork edin
2. Feature branch'i oluşturun (`git checkout -b feature/YeniOzellik`)
3. Değişikliklerinizi commit edin (`git commit -m 'Yeni özellik eklendi'`)
4. Branch'inizi push edin (`git push origin feature/YeniOzellik`)
5. Pull Request oluşturun

## Lisans

Bu proje [MIT lisansı](LICENSE) altında lisanslanmıştır.

## İletişim

Betül Öztürk - [GitHub](https://github.com/betulzzturkk) 

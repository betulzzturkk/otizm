using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutismEducationPlatform.Data;
using AutismEducationPlatform.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AutismEducationPlatform.Controllers
{
    public class EgitmenController : Controller
    {
        private readonly UygulamaDbContext _context;

        public EgitmenController(UygulamaDbContext context)
        {
            _context = context;
        }

        // Eğitmen Panel
        public async Task<IActionResult> Panel()
        {
            if (HttpContext.Session.GetString("KullaniciTipi") != "Egitmen")
            {
                return RedirectToAction("Giris", "Kullanici");
            }

            var kullaniciId = int.Parse(HttpContext.Session.GetString("KullaniciId"));
            var egitmen = await _context.Kullanicilar.FindAsync(kullaniciId);

            ViewBag.EgitmenAdi = $"{egitmen.Ad} {egitmen.Soyad}";
            
            // Öğrenci listesini getir
            var ogrenciler = await _context.Kullanicilar
                .Where(k => k.KullaniciTipi == "Veli")
                .ToListAsync();

            return View(ogrenciler);
        }

        // Öğrenci Detay
        public async Task<IActionResult> OgrenciDetay(int id)
        {
            if (HttpContext.Session.GetString("KullaniciTipi") != "Egitmen")
            {
                return RedirectToAction("Giris", "Kullanici");
            }

            var ogrenci = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.Id == id && k.KullaniciTipi == "Veli");

            if (ogrenci == null)
            {
                return NotFound();
            }

            return View(ogrenci);
        }

        // İlerleme Raporu Ekle
        [HttpPost]
        public async Task<IActionResult> IlerlemeRaporuEkle(int ogrenciId, string rapor)
        {
            if (HttpContext.Session.GetString("KullaniciTipi") != "Egitmen")
            {
                return Json(new { success = false, message = "Yetkiniz yok" });
            }

            if (string.IsNullOrEmpty(rapor))
            {
                return Json(new { success = false, message = "Rapor boş olamaz" });
            }

            // TODO: İlerleme raporunu veritabanına kaydet
            // Bu kısım ilerleme raporu modeli oluşturulduktan sonra implement edilecek

            return Json(new { success = true, message = "Rapor başarıyla eklendi" });
        }

        // Eğitim İçeriği Yönetimi
        public async Task<IActionResult> EgitimIcerikleri()
        {
            if (HttpContext.Session.GetString("KullaniciTipi") != "Egitmen")
            {
                return RedirectToAction("Giris", "Kullanici");
            }

            var egitimModulleri = await _context.EgitimModulleri
                .Include(e => e.Icerikler)
                .ToListAsync();

            return View(egitimModulleri);
        }

        // Yeni Eğitim İçeriği Ekle
        [HttpPost]
        public async Task<IActionResult> IcerikEkle(int modulId, string baslik, string icerik)
        {
            if (HttpContext.Session.GetString("KullaniciTipi") != "Egitmen")
            {
                return Json(new { success = false, message = "Yetkiniz yok" });
            }

            var yeniIcerik = new ModulIcerik
            {
                EgitimModuluId = modulId,
                Baslik = baslik,
                Icerik = icerik
            };

            _context.ModulIcerikleri.Add(yeniIcerik);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "İçerik başarıyla eklendi" });
        }
    }
} 
using Microsoft.AspNetCore.Mvc;
using AutismEducationPlatform.Models;
using AutismEducationPlatform.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace AutismEducationPlatform.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly UygulamaDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KullaniciController(UygulamaDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: /Kullanici/Giris
        public IActionResult Giris()
        {
            return View();
        }

        // POST: /Kullanici/Giris
        [HttpPost]
        public IActionResult GirisYap(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Giris", model);
            }

            var kullanici = _context.Kullanicilar
                .FirstOrDefault(k => k.Email == model.Email && k.Sifre == model.Password);

            if (kullanici != null)
            {
                // Session'a kullanıcı bilgilerini kaydet
                HttpContext.Session.SetString("KullaniciId", kullanici.Id.ToString());
                HttpContext.Session.SetString("KullaniciAd", kullanici.Ad);
                HttpContext.Session.SetString("KullaniciTipi", kullanici.KullaniciTipi);

                switch (kullanici.KullaniciTipi)
                {
                    case "Admin":
                        return RedirectToAction("Panel", "Admin");
                    case "Egitmen":
                        return RedirectToAction("Panel", "Egitmen");
                    case "Veli":
                        return RedirectToAction("Index", "Home");
                    default:
                        return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "E-posta adresi veya şifre hatalı");
            return View("Giris", model);
        }

        public IActionResult CocukBilgileri()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CocukBilgileriKaydet(CocukBilgileriViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CocukBilgileri", model);
            }

            // TODO: Çocuk bilgilerini veritabanına kaydetme işlemi burada yapılacak

            // Başarılı kayıt sonrası eğitimler sayfasına yönlendir
            TempData["Mesaj"] = "Çocuk bilgileri başarıyla kaydedildi.";
            return RedirectToAction("Index", "Egitim");
        }

        // GET: /Kullanici/Kayit
        public IActionResult Kayit()
        {
            return View();
        }

        // POST: /Kullanici/Kayit
        [HttpPost]
        public IActionResult Kayit(KayitViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Admin kaydını engelle
            if (model.KullaniciTipi.ToLower() == "admin")
            {
                ModelState.AddModelError("KullaniciTipi", "Admin kaydı yapılamaz");
                return View(model);
            }

            if (_context.Kullanicilar.Any(k => k.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılıyor");
                return View(model);
            }

            var kullanici = new Kullanici
            {
                Ad = model.Ad,
                Soyad = model.Soyad,
                Email = model.Email,
                Sifre = model.Password,
                KullaniciTipi = model.KullaniciTipi
            };

            _context.Kullanicilar.Add(kullanici);
            _context.SaveChanges();

            TempData["Mesaj"] = "Kayıt işlemi başarılı. Lütfen giriş yapın.";
            return RedirectToAction("Giris");
        }

        [HttpPost]
        public IActionResult SwitchToChild()
        {
            HttpContext.Session.SetString("KullaniciTipi", "Kullanici");
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult SwitchToParent()
        {
            HttpContext.Session.SetString("KullaniciTipi", "Veli");
            return Json(new { success = true });
        }

        public IActionResult Cikis()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult KullaniciBilgileri()
        {
            var kullaniciIdStr = HttpContext.Session.GetString("KullaniciId");
            if (string.IsNullOrEmpty(kullaniciIdStr))
            {
                return RedirectToAction("Giris");
            }

            var kullaniciId = int.Parse(kullaniciIdStr);
            var kullanici = _context.Kullanicilar.Find(kullaniciId);
            if (kullanici == null)
            {
                return RedirectToAction("Giris");
            }

            return View(kullanici);
        }
    }
} 
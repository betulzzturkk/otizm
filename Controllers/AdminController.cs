using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutismEducationPlatform.Data;
using AutismEducationPlatform.Models;
using AutismEducationPlatform.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace AutismEducationPlatform.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class AdminController : Controller
    {
        private readonly UygulamaDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static readonly Dictionary<string, (int Count, DateTime LastAttempt)> _loginAttempts = new();
        private const int MaxLoginAttempts = 5;
        private const int LockoutMinutes = 15;

        public AdminController(UygulamaDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private bool IsIpBlocked(string ipAddress)
        {
            if (_loginAttempts.TryGetValue(ipAddress, out var attempts))
            {
                if (attempts.Count >= MaxLoginAttempts && 
                    DateTime.Now.Subtract(attempts.LastAttempt).TotalMinutes < LockoutMinutes)
                {
                    return true;
                }
                else if (DateTime.Now.Subtract(attempts.LastAttempt).TotalMinutes >= LockoutMinutes)
                {
                    _loginAttempts.Remove(ipAddress);
                }
            }
            return false;
        }

        private void RecordLoginAttempt(string ipAddress)
        {
            if (_loginAttempts.TryGetValue(ipAddress, out var attempts))
            {
                _loginAttempts[ipAddress] = (attempts.Count + 1, DateTime.Now);
            }
            else
            {
                _loginAttempts[ipAddress] = (1, DateTime.Now);
            }
        }

        // Admin Giriş Sayfası
        public IActionResult Giris()
        {
            var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            
            if (IsIpBlocked(ipAddress))
            {
                ModelState.AddModelError("", $"Çok fazla başarısız giriş denemesi. Lütfen {LockoutMinutes} dakika bekleyin.");
                return View();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Giris(string kullaniciAdi, string sifre)
        {
            var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            
            if (IsIpBlocked(ipAddress))
            {
                ModelState.AddModelError("", $"Çok fazla başarısız giriş denemesi. Lütfen {LockoutMinutes} dakika bekleyin.");
                return View();
            }

            var admin = await _context.Kullanicilar
                .FirstOrDefaultAsync(a => a.KullaniciAdi == kullaniciAdi && a.KullaniciTipi == "Admin");

            if (admin != null && PasswordHasher.VerifyPassword(admin.Sifre, sifre))
            {
                _httpContextAccessor.HttpContext?.Session.SetString("AdminId", admin.Id.ToString());
                _loginAttempts.Remove(ipAddress); // Başarılı girişte deneme sayısını sıfırla
                return RedirectToAction("Panel");
            }

            RecordLoginAttempt(ipAddress);
            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
            return View();
        }

        // Admin Panel - Yetkilendirme kontrolü
        [AdminAuth]
        public IActionResult Panel()
        {
            var viewModel = new AdminPanelViewModel
            {
                Veliler = _context.Kullanicilar.Where(k => k.KullaniciTipi == "Veli").ToList(),
                Cocuklar = _context.Cocuklar.Include(c => c.Veli).ToList(),
                EgitimModulleri = _context.EgitimModulleri.ToList(),
                CocukDurumlari = _context.CocukDurumlari.Include(cd => cd.Cocuk).ToList()
            };

            return View(viewModel);
        }

        [AdminAuth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EgitimModuluEkle(EgitimModulu model)
        {
            if (ModelState.IsValid)
            {
                await _context.EgitimModulleri.AddAsync(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Panel));
            }
            return RedirectToAction(nameof(Panel));
        }

        [AdminAuth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EgitimModuluSil(int id)
        {
            var modul = await _context.EgitimModulleri.FindAsync(id);
            if (modul != null)
            {
                _context.EgitimModulleri.Remove(modul);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Panel));
        }

        [AdminAuth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CocukDurumuEkle(CocukDurumu durum)
        {
            if (ModelState.IsValid)
            {
                _context.CocukDurumlari.Add(durum);
                _context.SaveChanges();

                var cocuk = _context.Cocuklar
                    .Include(c => c.Veli)
                    .FirstOrDefault(c => c.Id == durum.CocukId);

                if (cocuk?.Veli != null)
                {
                    var bildirim = new VeliBilgilendirme
                    {
                        VeliId = cocuk.VeliId,
                        Mesaj = $"{cocuk.Ad} {cocuk.Soyad} için yeni durum güncellendi: {durum.Durum}",
                        TarihSaat = DateTime.Now
                    };

                    _context.VeliBilgilendirmeler.Add(bildirim);
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Panel));
            }

            return RedirectToAction(nameof(Panel));
        }

        [AdminAuth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VeliyeBilgilendirmeGonder(int veliId, string mesaj)
        {
            var bilgilendirme = new VeliBilgilendirme
            {
                VeliId = veliId,
                Mesaj = mesaj,
                TarihSaat = DateTime.Now,
                Okundu = false
            };

            await _context.VeliBilgilendirmeler.AddAsync(bilgilendirme);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Panel));
        }

        public IActionResult Cikis()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
            return RedirectToAction("Giris", "Kullanici");
        }
    }

    // Admin yetkilendirme attribute'u
    public class AdminAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var adminId = httpContext.Session.GetString("AdminId");

            if (string.IsNullOrEmpty(adminId))
            {
                context.Result = new RedirectToActionResult("Giris", "Admin", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
} 
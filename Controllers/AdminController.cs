using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutismEducationPlatform.Data;
using AutismEducationPlatform.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AutismEducationPlatform.Controllers
{
    public class AdminController : Controller
    {
        private readonly UygulamaDbContext _context;

        public AdminController(UygulamaDbContext context)
        {
            _context = context;
        }

        // Admin Giriş Sayfası
        public IActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Giris(string kullaniciAdi, string sifre)
        {
            var admin = await _context.Adminler
                .FirstOrDefaultAsync(a => a.KullaniciAdi == kullaniciAdi && a.Sifre == sifre);

            if (admin != null)
            {
                HttpContext.Session.SetString("AdminId", admin.Id.ToString());
                return RedirectToAction("Panel");
            }

            ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
            return View();
        }

        // Admin Panel
        public async Task<IActionResult> Panel()
        {
            if (HttpContext.Session.GetString("AdminId") == null)
            {
                return RedirectToAction("Giris");
            }

            var kullanicilar = await _context.Kullanicilar.ToListAsync();
            return View(kullanicilar);
        }

        // Admin Çıkış
        public IActionResult Cikis()
        {
            HttpContext.Session.Remove("AdminId");
            return RedirectToAction("Giris");
        }
    }
} 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutismEducationPlatform.Data;
using AutismEducationPlatform.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AutismEducationPlatform.Controllers
{
    public class EgitimModuluController : Controller
    {
        private readonly UygulamaDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EgitimModuluController(UygulamaDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: EgitimModulu
        public IActionResult Index()
        {
            var moduller = _context.EgitimModulleri.ToList();
            return View(moduller);
        }

        // GET: EgitimModulu/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EgitimModulu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EgitimModulu modul, IFormFile resimDosyasi, IFormFile sesDosyasi)
        {
            if (ModelState.IsValid)
            {
                if (resimDosyasi != null)
                {
                    string resimKlasoru = Path.Combine(_hostingEnvironment.WebRootPath, "resimler");
                    if (!Directory.Exists(resimKlasoru))
                    {
                        Directory.CreateDirectory(resimKlasoru);
                    }

                    string benzersizResimAdi = Guid.NewGuid().ToString() + "_" + resimDosyasi.FileName;
                    string resimYolu = Path.Combine(resimKlasoru, benzersizResimAdi);

                    using (var stream = new FileStream(resimYolu, FileMode.Create))
                    {
                        await resimDosyasi.CopyToAsync(stream);
                    }

                    modul.ResimYolu = "/resimler/" + benzersizResimAdi;
                }

                if (sesDosyasi != null)
                {
                    string sesKlasoru = Path.Combine(_hostingEnvironment.WebRootPath, "sesler");
                    if (!Directory.Exists(sesKlasoru))
                    {
                        Directory.CreateDirectory(sesKlasoru);
                    }

                    string benzersizSesAdi = Guid.NewGuid().ToString() + "_" + sesDosyasi.FileName;
                    string sesYolu = Path.Combine(sesKlasoru, benzersizSesAdi);

                    using (var stream = new FileStream(sesYolu, FileMode.Create))
                    {
                        await sesDosyasi.CopyToAsync(stream);
                    }

                    modul.SesYolu = "/sesler/" + benzersizSesAdi;
                }

                _context.Add(modul);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modul);
        }

        // GET: EgitimModulu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modul = await _context.EgitimModulleri.FindAsync(id);
            if (modul == null)
            {
                return NotFound();
            }
            return View(modul);
        }

        // POST: EgitimModulu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EgitimModulu modul, IFormFile resimDosyasi, IFormFile sesDosyasi)
        {
            if (id != modul.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var mevcutModul = await _context.EgitimModulleri.FindAsync(id);
                    if (mevcutModul == null)
                    {
                        return NotFound();
                    }

                    // Resim güncelleme
                    if (resimDosyasi != null)
                    {
                        // Eski resmi sil
                        if (!string.IsNullOrEmpty(mevcutModul.ResimYolu))
                        {
                            var eskiResimYolu = Path.Combine(_hostingEnvironment.WebRootPath, mevcutModul.ResimYolu.TrimStart('/'));
                            if (System.IO.File.Exists(eskiResimYolu))
                            {
                                System.IO.File.Delete(eskiResimYolu);
                            }
                        }

                        // Yeni resmi kaydet
                        string resimKlasoru = Path.Combine(_hostingEnvironment.WebRootPath, "resimler");
                        string benzersizResimAdi = Guid.NewGuid().ToString() + "_" + resimDosyasi.FileName;
                        string resimYolu = Path.Combine(resimKlasoru, benzersizResimAdi);

                        using (var stream = new FileStream(resimYolu, FileMode.Create))
                        {
                            await resimDosyasi.CopyToAsync(stream);
                        }

                        mevcutModul.ResimYolu = "/resimler/" + benzersizResimAdi;
                    }

                    // Ses güncelleme
                    if (sesDosyasi != null)
                    {
                        // Eski sesi sil
                        if (!string.IsNullOrEmpty(mevcutModul.SesYolu))
                        {
                            var eskiSesYolu = Path.Combine(_hostingEnvironment.WebRootPath, mevcutModul.SesYolu.TrimStart('/'));
                            if (System.IO.File.Exists(eskiSesYolu))
                            {
                                System.IO.File.Delete(eskiSesYolu);
                            }
                        }

                        // Yeni sesi kaydet
                        string sesKlasoru = Path.Combine(_hostingEnvironment.WebRootPath, "sesler");
                        string benzersizSesAdi = Guid.NewGuid().ToString() + "_" + sesDosyasi.FileName;
                        string sesYolu = Path.Combine(sesKlasoru, benzersizSesAdi);

                        using (var stream = new FileStream(sesYolu, FileMode.Create))
                        {
                            await sesDosyasi.CopyToAsync(stream);
                        }

                        mevcutModul.SesYolu = "/sesler/" + benzersizSesAdi;
                    }

                    // Diğer özellikleri güncelle
                    mevcutModul.ModulAdi = modul.ModulAdi;
                    mevcutModul.ModulTipi = modul.ModulTipi;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModulExists(modul.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(modul);
        }

        // GET: EgitimModulu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modul = await _context.EgitimModulleri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (modul == null)
            {
                return NotFound();
            }

            return View(modul);
        }

        // POST: EgitimModulu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modul = await _context.EgitimModulleri.FindAsync(id);
            if (modul != null)
            {
                // Resim ve ses dosyalarını sil
                if (!string.IsNullOrEmpty(modul.ResimYolu))
                {
                    var resimYolu = Path.Combine(_hostingEnvironment.WebRootPath, modul.ResimYolu.TrimStart('/'));
                    if (System.IO.File.Exists(resimYolu))
                    {
                        System.IO.File.Delete(resimYolu);
                    }
                }

                if (!string.IsNullOrEmpty(modul.SesYolu))
                {
                    var sesYolu = Path.Combine(_hostingEnvironment.WebRootPath, modul.SesYolu.TrimStart('/'));
                    if (System.IO.File.Exists(sesYolu))
                    {
                        System.IO.File.Delete(sesYolu);
                    }
                }

                _context.EgitimModulleri.Remove(modul);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ModulExists(int id)
        {
            return _context.EgitimModulleri.Any(e => e.Id == id);
        }
    }
} 
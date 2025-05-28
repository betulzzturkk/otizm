using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutismEducationPlatform.Data;
using AutismEducationPlatform.Models;

namespace AutismEducationPlatform.Controllers
{
    public class EgitimModuluController : Controller
    {
        private readonly UygulamaDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EgitimModuluController(UygulamaDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: EgitimModulu
        public async Task<IActionResult> Index()
        {
            var moduller = await _context.EgitimModulleri.ToListAsync();
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
        public async Task<IActionResult> Create(EgitimModulu egitimModulu, IFormFile Resim, IFormFile Ses)
        {
            if (ModelState.IsValid)
            {
                if (Resim != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(Resim.FileName);
                    string extension = Path.GetExtension(Resim.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/images/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await Resim.CopyToAsync(fileStream);
                    }
                    egitimModulu.ResimYolu = "/images/" + fileName;
                }

                if (Ses != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(Ses.FileName);
                    string extension = Path.GetExtension(Ses.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/sounds/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await Ses.CopyToAsync(fileStream);
                    }
                    egitimModulu.SesYolu = "/sounds/" + fileName;
                }

                _context.Add(egitimModulu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(egitimModulu);
        }

        // GET: EgitimModulu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var egitimModulu = await _context.EgitimModulleri.FindAsync(id);
            if (egitimModulu == null)
            {
                return NotFound();
            }
            return View(egitimModulu);
        }

        // POST: EgitimModulu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EgitimModulu egitimModulu, IFormFile Resim, IFormFile Ses)
        {
            if (id != egitimModulu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingModul = await _context.EgitimModulleri.FindAsync(id);
                    if (existingModul == null)
                    {
                        return NotFound();
                    }

                    if (Resim != null)
                    {
                        // Eski resmi sil
                        if (!string.IsNullOrEmpty(existingModul.ResimYolu))
                        {
                            var oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, existingModul.ResimYolu.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Yeni resmi kaydet
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(Resim.FileName);
                        string extension = Path.GetExtension(Resim.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/images/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await Resim.CopyToAsync(fileStream);
                        }
                        existingModul.ResimYolu = "/images/" + fileName;
                    }

                    if (Ses != null)
                    {
                        // Eski ses dosyasını sil
                        if (!string.IsNullOrEmpty(existingModul.SesYolu))
                        {
                            var oldSoundPath = Path.Combine(_hostEnvironment.WebRootPath, existingModul.SesYolu.TrimStart('/'));
                            if (System.IO.File.Exists(oldSoundPath))
                            {
                                System.IO.File.Delete(oldSoundPath);
                            }
                        }

                        // Yeni ses dosyasını kaydet
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(Ses.FileName);
                        string extension = Path.GetExtension(Ses.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/sounds/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await Ses.CopyToAsync(fileStream);
                        }
                        existingModul.SesYolu = "/sounds/" + fileName;
                    }

                    existingModul.Ad = egitimModulu.Ad;
                    existingModul.Aciklama = egitimModulu.Aciklama;
                    existingModul.ModulTipi = egitimModulu.ModulTipi;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EgitimModuluExists(egitimModulu.Id))
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
            return View(egitimModulu);
        }

        // GET: EgitimModulu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var egitimModulu = await _context.EgitimModulleri
                .FirstOrDefaultAsync(m => m.Id == id);
            if (egitimModulu == null)
            {
                return NotFound();
            }

            return View(egitimModulu);
        }

        // POST: EgitimModulu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var egitimModulu = await _context.EgitimModulleri.FindAsync(id);
            if (egitimModulu != null)
            {
                // Resim ve ses dosyalarını sil
                if (!string.IsNullOrEmpty(egitimModulu.ResimYolu))
                {
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, egitimModulu.ResimYolu.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                if (!string.IsNullOrEmpty(egitimModulu.SesYolu))
                {
                    var soundPath = Path.Combine(_hostEnvironment.WebRootPath, egitimModulu.SesYolu.TrimStart('/'));
                    if (System.IO.File.Exists(soundPath))
                    {
                        System.IO.File.Delete(soundPath);
                    }
                }

                _context.EgitimModulleri.Remove(egitimModulu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EgitimModuluExists(int id)
        {
            return _context.EgitimModulleri.Any(e => e.Id == id);
        }
    }
} 
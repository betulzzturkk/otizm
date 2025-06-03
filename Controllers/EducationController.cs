using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AutismEducationPlatform.Data;
using AutismEducationPlatform.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace AutismEducationPlatform.Controllers
{
    public class EducationController : Controller
    {
        private readonly UygulamaDbContext _context;

        public EducationController(UygulamaDbContext context)
        {
            _context = context;
        }

        // Eğitim modülleri ana sayfası
        public IActionResult Index()
        {
            return View();
        }

        // Hayvanlar modülü
        public async Task<IActionResult> Animals()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userId))
            {
                var childId = await _context.Children
                    .Where(c => c.ParentId == int.Parse(userId))
                    .Select(c => c.Id)
                    .FirstOrDefaultAsync();

                if (childId != 0)
                {
                    var progress = await _context.LearningProgress
                        .Where(p => p.ChildId == childId && p.Module.Name == "Animals")
                        .FirstOrDefaultAsync();
                    ViewBag.Progress = progress;
                }
            }
            return View();
        }

        // İlerleme kaydetme - Sadece giriş yapmış kullanıcılar için
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveProgress([FromBody] LearningProgress progress)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User not found");
            }

            var childId = await _context.Children
                .Where(c => c.ParentId == int.Parse(userId))
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (childId == 0)
            {
                return BadRequest("Child not found");
            }

            var existingProgress = await _context.LearningProgress
                .FirstOrDefaultAsync(p => p.ChildId == childId && 
                                        p.ModuleId == progress.ModuleId);

            if (existingProgress == null)
            {
                progress.ChildId = childId;
                progress.CreatedAt = DateTime.UtcNow;
                progress.LastAccessDate = DateTime.UtcNow;
                _context.LearningProgress.Add(progress);
            }
            else
            {
                existingProgress.CompletedContentCount = progress.CompletedContentCount;
                existingProgress.ProgressPercentage = progress.ProgressPercentage;
                existingProgress.LastAccessDate = DateTime.UtcNow;
                _context.LearningProgress.Update(existingProgress);
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        // İlerleme durumunu getirme - Sadece giriş yapmış kullanıcılar için
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetProgress(string moduleName)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return Json(new { success = false, message = "Oturum bulunamadı" });

            var childId = await _context.Children
                .Where(c => c.ParentId == int.Parse(userId))
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            var progress = await _context.LearningProgress
                .Include(lp => lp.Module)
                .Where(lp => lp.ChildId == childId && lp.Module.Name == moduleName)
                .Select(lp => new { 
                    CompletedContent = lp.CompletedContentCount,
                    TotalContent = lp.TotalContentCount,
                    Progress = lp.ProgressPercentage
                })
                .FirstOrDefaultAsync();

            return Json(progress);
        }

        // Sayılar modülü
        public IActionResult Numbers() => View();

        // Renkler modülü
        public IActionResult Colors() => View();

        // Şekiller modülü
        public IActionResult Shapes() => View();

        // Trafik İşaretleri modülü
        public IActionResult TrafficSigns() => View();

        // Görgü Kuralları modülü
        public IActionResult GorguKurallari()
        {
            return View("Manners");
        }

        // Eğitim Videoları modülü
        public IActionResult Videos() => View();

        // Masallar modülü
        public IActionResult Stories() => View();

        // Mannerler modülü
        public IActionResult Manners() => View();

        [HttpPost]
        public async Task<IActionResult> UpdateProgress(string moduleName, string contentName)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return Json(new { success = false, message = "Oturum bulunamadı" });

            var childId = await _context.Children
                .Where(c => c.ParentId == int.Parse(userId))
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            var module = await _context.LearningModules
                .Include(m => m.Contents)
                .FirstOrDefaultAsync(m => m.Name == moduleName);

            if (module == null) return Json(new { success = false, message = "Modül bulunamadı" });

            var progress = await _context.LearningProgress
                .FirstOrDefaultAsync(lp => lp.ChildId == childId && lp.ModuleId == module.Id);

            if (progress == null)
            {
                progress = new LearningProgress
                {
                    ChildId = childId,
                    ModuleId = module.Id,
                    CompletedContentCount = 1,
                    TotalContentCount = module.Contents.Count,
                    ProgressPercentage = (decimal)1 / module.Contents.Count * 100,
                    LastAccessDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };
                _context.LearningProgress.Add(progress);
            }
            else
            {
                progress.CompletedContentCount++;
                progress.ProgressPercentage = (decimal)progress.CompletedContentCount / progress.TotalContentCount * 100;
                progress.LastAccessDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return Json(new { 
                success = true, 
                completedContent = progress.CompletedContentCount,
                totalContent = progress.TotalContentCount,
                progress = progress.ProgressPercentage
            });
        }
    }
} 
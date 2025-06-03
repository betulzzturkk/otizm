using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutismEducationPlatform.Data;
using AutismEducationPlatform.Models;
using AutismEducationPlatform.Models.ViewModels;
using AutismEducationPlatform.Helpers;
using Microsoft.AspNetCore.Http;

namespace AutismEducationPlatform.Controllers
{
    public class ParentController : Controller
    {
        private readonly UygulamaDbContext _context;

        public ParentController(UygulamaDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email && !u.IsAdmin);
                
                if (user != null && PasswordHasher.VerifyPassword(model.Password, user.PasswordHash))
                {
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");

                    // Çocuk bilgileri kontrol edilir
                    var hasChildInfo = await _context.Children.AnyAsync(c => c.ParentId == user.Id);
                    if (!hasChildInfo)
                    {
                        return RedirectToAction("AddChildInformation");
                    }

                    return RedirectToAction("Dashboard");
                }

                ModelState.AddModelError("", "Geçersiz e-posta veya şifre");
            }

            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Models.ViewModels.ParentRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılıyor");
                    return View(model);
                }

                var hashedPassword = PasswordHasher.HashPassword(model.Password);
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PasswordHash = hashedPassword,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    IsAdmin = false,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        public IActionResult AddChildInformation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddChildInformation(Models.ViewModels.ChildInformationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var child = new Child
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    DiagnosisDate = model.DiagnosisDate,
                    SpecialNotes = model.SpecialNotes,
                    Medications = model.Medications,
                    Allergies = model.Allergies,
                    EducationalHistory = model.EducationalHistory,
                    ParentId = userId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Children.Add(child);
                await _context.SaveChangesAsync();

                return RedirectToAction("Dashboard");
            }

            return View(model);
        }

        public async Task<IActionResult> Dashboard()
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));
            var user = await _context.Users
                .Include(u => u.Children)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return View(user);
        }

        public async Task<IActionResult> ToggleChildMode()
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));
            var user = await _context.Users.FindAsync(userId);
            
            if (user != null)
            {
                user.IsInChildMode = !user.IsInChildMode;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> News()
        {
            var news = await _context.News.OrderByDescending(n => n.CreatedAt).ToListAsync();
            return View(news);
        }

        public async Task<IActionResult> Reports()
        {
            var userId = int.Parse(HttpContext.Session.GetString("UserId"));
            var childProgress = await _context.Children
                .Where(c => c.ParentId == userId)
                .Include(c => c.LearningProgress)
                .ThenInclude(lp => lp.Module)
                .ToListAsync();

            return View(childProgress);
        }
    }
} 
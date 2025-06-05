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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AutismEducationPlatform.Controllers
{
    [Authorize(Roles = "Parent")]
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

        [Authorize(Policy = "ParentOnly")]
        public async Task<IActionResult> Dashboard()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parentId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var parent = await _context.Parents
                .Include(p => p.Children)
                .FirstOrDefaultAsync(p => p.Id == parentId);

            if (parent == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(parent);
        }

        [Authorize(Policy = "ParentOnly")]
        public IActionResult AddChildInformation()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "ParentOnly")]
        public async Task<IActionResult> AddChildInformation(Models.ViewModels.ChildInformationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parentId))
                {
                    return RedirectToAction("Login", "Auth");
                }

                var child = new Child
                {
                    FirstName = model.FirstName ?? string.Empty,
                    LastName = model.LastName ?? string.Empty,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender ?? string.Empty,
                    DiagnosisDate = model.DiagnosisDate,
                    SpecialNotes = model.SpecialNotes ?? string.Empty,
                    Medications = model.Medications ?? string.Empty,
                    Allergies = model.Allergies ?? string.Empty,
                    EducationalHistory = model.EducationalHistory ?? string.Empty,
                    ParentId = parentId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Children.Add(child);
                await _context.SaveChangesAsync();

                return RedirectToAction("Dashboard");
            }

            return View(model);
        }

        public async Task<IActionResult> ToggleChildMode()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return RedirectToAction("Login");
            }

            var user = await _context.Users.FindAsync(userId);
            
            if (user != null)
            {
                user.IsInChildMode = !user.IsInChildMode;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Policy = "ParentOnly")]
        public async Task<IActionResult> News()
        {
            var news = await _context.News.OrderByDescending(n => n.CreatedAt).ToListAsync();
            return View(news);
        }

        [Authorize(Policy = "ParentOnly")]
        public async Task<IActionResult> Reports()
        {
            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parentId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var childProgress = await _context.Children
                .Where(c => c.ParentId == parentId)
                .Include(c => c.LearningProgresses)
                .ThenInclude(lp => lp.Module)
                .ToListAsync();

            return View(childProgress);
        }

        public IActionResult Panel()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var parent = _context.Parents
                .Include(p => p.User)
                .Include(p => p.Children)
                .ThenInclude(c => c.Educations)
                .ThenInclude(e => e.Instructor)
                .ThenInclude(i => i.User)
                .FirstOrDefault(p => p.UserId == userId);

            if (parent == null)
            {
                return NotFound();
            }

            return View(parent);
        }

        public IActionResult ChildDetails(int id)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var child = _context.Children
                .Include(c => c.Parent)
                .ThenInclude(p => p.User)
                .Include(c => c.Educations)
                .ThenInclude(e => e.Instructor)
                .ThenInclude(i => i.User)
                .Include(c => c.ProgressReports)
                .Include(c => c.StatusHistory)
                .Include(c => c.ParentInformations)
                .Include(c => c.LearningProgresses)
                .FirstOrDefault(c => c.Id == id && c.Parent.UserId == userId);

            if (child == null)
            {
                return NotFound();
            }

            return View(child);
        }

        public IActionResult EducationDetails(int id)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var education = _context.Educations
                .Include(e => e.Child)
                .ThenInclude(c => c.Parent)
                .Include(e => e.Instructor)
                .ThenInclude(i => i.User)
                .Include(e => e.Modules)
                .ThenInclude(m => m.Contents)
                .FirstOrDefault(e => e.Id == id && e.Child.Parent.UserId == userId);

            if (education == null)
            {
                return NotFound();
            }

            return View(education);
        }

        public IActionResult Profile()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var parent = _context.Parents
                .Include(p => p.User)
                .FirstOrDefault(p => p.UserId == userId);

            if (parent == null)
            {
                return NotFound();
            }

            return View(parent);
        }

        [HttpPost]
        public IActionResult UpdateProfile(Parent parent)
        {
            if (!ModelState.IsValid)
            {
                return View("Profile", parent);
            }

            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var existingParent = _context.Parents
                .Include(p => p.User)
                .FirstOrDefault(p => p.UserId == userId);

            if (existingParent == null)
            {
                return NotFound();
            }

            existingParent.User.FirstName = parent.User.FirstName;
            existingParent.User.LastName = parent.User.LastName;
            existingParent.User.PhoneNumber = parent.User.PhoneNumber;
            existingParent.User.Address = parent.User.Address;
            existingParent.User.LastUpdatedAt = DateTime.Now;

            existingParent.Occupation = parent.Occupation;
            existingParent.City = parent.City;
            existingParent.EmergencyContact = parent.EmergencyContact;
            existingParent.LastUpdatedAt = DateTime.Now;

            _context.SaveChanges();

            TempData["Success"] = "Profiliniz başarıyla güncellendi.";
            return RedirectToAction("Profile");
        }
    }
} 
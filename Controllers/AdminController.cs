using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;
using AutismEducationPlatform.Models;
using AutismEducationPlatform.Data;
using AutismEducationPlatform.Helpers;
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

        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            if (ModelState.IsValid && model.Email != null && model.Password != null)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.IsAdmin);

                if (user != null && user.PasswordHash != null && 
                    PasswordHasher.VerifyPassword(model.Password, user.PasswordHash))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim("UserId", user.Id.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction("Index");
                }

                ModelState.AddModelError("", "Geçersiz e-posta veya şifre");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult Index()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        public async Task<IActionResult> Panel()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null || !user.IsAdmin) return RedirectToAction("AccessDenied", "Account");

            var userCount = await _context.Users.CountAsync();
            var childCount = await _context.Children.CountAsync();
            var moduleCount = await _context.LearningModules.CountAsync();
            var newsCount = await _context.News.CountAsync();

            ViewBag.UserCount = userCount;
            ViewBag.ChildCount = childCount;
            ViewBag.ModuleCount = moduleCount;
            ViewBag.NewsCount = newsCount;

            return View();
        }

        public async Task<IActionResult> Users()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null || !user.IsAdmin) return RedirectToAction("AccessDenied", "Account");

            var users = await _context.Users
                .Include(u => u.Children)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> Children()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null || !user.IsAdmin) return RedirectToAction("AccessDenied", "Account");

            var children = await _context.Children
                .Include(c => c.Parent)
                .Include(c => c.LearningProgresses)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(children);
        }

        public async Task<IActionResult> Modules()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null || !user.IsAdmin) return RedirectToAction("AccessDenied", "Account");

            var modules = await _context.LearningModules
                .Include(m => m.Contents)
                .OrderBy(m => m.OrderIndex)
                .ToListAsync();

            return View(modules);
        }

        public async Task<IActionResult> News()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FindAsync(int.Parse(userId));
            if (user == null || !user.IsAdmin) return RedirectToAction("AccessDenied", "Account");

            var news = await _context.News
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return View(news);
        }

        public async Task<IActionResult> GetUserCount()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return Json(new { count = 0 });
            }

            var count = await _context.Users.CountAsync(u => !u.IsAdmin);
            return Json(new { count });
        }
    }
} 
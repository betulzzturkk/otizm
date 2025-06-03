using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutismEducationPlatform.Models;
using AutismEducationPlatform.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using AutismEducationPlatform.Models.ViewModels;
using AutismEducationPlatform.Helpers;

namespace AutismEducationPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly UygulamaDbContext _context;

        public AccountController(UygulamaDbContext context)
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
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                
                if (user != null && PasswordHasher.VerifyPassword(model.Password, user.PasswordHash))
                {
                    HttpContext.Session.SetString("UserId", user.Id.ToString());
                    HttpContext.Session.SetString("UserEmail", user.Email);
                    HttpContext.Session.SetString("UserName", $"{user.FirstName} {user.LastName}");
                    HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());

                    if (user.IsAdmin)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "Parent");
                    }
                }

                ModelState.AddModelError("", "Geçersiz e-posta veya şifre");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılıyor");
                    return View(model);
                }

                model.PasswordHash = PasswordHasher.HashPassword(model.Password);
                model.CreatedAt = DateTime.UtcNow;

                _context.Users.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Login));
            }
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
} 
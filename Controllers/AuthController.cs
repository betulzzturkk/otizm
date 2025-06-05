using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using AutismEducationPlatform.Models;
using AutismEducationPlatform.Data;
using System.Security.Cryptography;
using System.Text;
using AutismEducationPlatform.Models.ViewModels;
using System.Linq;

namespace AutismEducationPlatform.Controllers
{
    public class AuthController : Controller
    {
        private readonly UygulamaDbContext _context;

        public AuthController(UygulamaDbContext context)
        {
            _context = context;
        }

        // Kayıt sayfalarını göster
        public IActionResult ParentRegister() => View();
        public IActionResult InstructorRegister() => View();
        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        public async Task<IActionResult> ParentRegister(Parent model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Parents.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Bu e-posta adresi zaten kullanımda.");
                    return View(model);
                }

                if (string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("Password", "Şifre alanı zorunludur.");
                    return View(model);
                }

                model.Password = HashPassword(model.Password);
                _context.Parents.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> InstructorRegister(Instructor model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Instructors.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Bu e-posta adresi zaten kullanımda.");
                    return View(model);
                }

                if (string.IsNullOrEmpty(model.Password))
                {
                    ModelState.AddModelError("Password", "Şifre alanı zorunludur.");
                    return View(model);
                }

                model.Password = HashPassword(model.Password);
                _context.Instructors.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.Users
                .FirstOrDefault(u => u.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Email veya şifre hatalı.");
                return View(model);
            }

            if (!user.IsActive)
            {
                ModelState.AddModelError("", "Hesabınız aktif değil.");
                return View(model);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim("UserId", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.UserType.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(7) : null
            };

            HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties
            ).Wait();

            user.LastLoginAt = DateTime.Now;
            _context.SaveChanges();

            switch (user.UserType)
            {
                case UserType.Admin:
                    return RedirectToAction("Index", "Admin");
                case UserType.Parent:
                    return RedirectToAction("Panel", "Parent");
                case UserType.Instructor:
                    return RedirectToAction("Panel", "Instructor");
                default:
                    return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_context.Users.Any(u => u.Email == model.Email))
            {
                ModelState.AddModelError("Email", "Bu email adresi zaten kullanılıyor.");
                return View(model);
            }

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                IsActive = true,
                UserType = UserType.Parent,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            var parent = new Parent
            {
                UserId = user.Id,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _context.Parents.Add(parent);
            _context.SaveChanges();

            TempData["Success"] = "Kayıt başarılı. Lütfen giriş yapın.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return RedirectToAction("Index", "Home");
        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Password cannot be null or empty");
            }

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
} 
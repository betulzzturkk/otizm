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
            return RedirectToAction("Login", "Auth");
        }

        public IActionResult Register()
        {
            return RedirectToAction("ParentRegister", "Auth");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
} 
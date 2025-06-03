using Microsoft.AspNetCore.Mvc;
using AutismEducationPlatform.Data;
using AutismEducationPlatform.Models;

namespace AutismEducationPlatform.Controllers;

public class HomeController : Controller
{
    private readonly UygulamaDbContext _context;

    public HomeController(UygulamaDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // Eğer kullanıcı giriş yapmışsa
        if (User.Identity?.IsAuthenticated == true)
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (User.IsInRole("Parent"))
            {
                return RedirectToAction("Index", "Parent");
            }
            else if (User.IsInRole("Instructor"))
            {
                return RedirectToAction("Index", "Instructor");
            }
        }

        // Giriş yapmamış kullanıcılar için hoşgeldiniz sayfası
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult FAQ()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SaveMessage([FromBody] Message message)
    {
        if (ModelState.IsValid)
        {
            message.CreatedAt = DateTime.Now;
            message.IsRead = false;
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
        return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}

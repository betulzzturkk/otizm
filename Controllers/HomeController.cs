using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AutismEducationPlatform.Models;
using AutismEducationPlatform.Data;

namespace AutismEducationPlatform.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UygulamaDbContext _context;

    public HomeController(ILogger<HomeController> logger, UygulamaDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
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
    public async Task<IActionResult> SaveMessage([FromBody] ContactViewModel model)
    {
        if (ModelState.IsValid)
        {
            var message = new Message
            {
                SenderName = model.Name,
                Email = model.Email,
                Subject = model.Subject,
                Content = model.Message,
                CreatedAt = DateTime.Now,
                IsRead = false
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
        return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors) });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

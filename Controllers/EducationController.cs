using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutismEducationPlatform.Models;
using AutismEducationPlatform.Data;
using System.Security.Claims;

namespace AutismEducationPlatform.Controllers
{
    [Authorize(Roles = "Instructor")]
    public class EducationController : Controller
    {
        private readonly UygulamaDbContext _context;

        public EducationController(UygulamaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var instructor = _context.Instructors
                .Include(i => i.User)
                .Include(i => i.Educations)
                .ThenInclude(e => e.Child)
                .ThenInclude(c => c.Parent)
                .ThenInclude(p => p.User)
                .FirstOrDefault(i => i.UserId == userId);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor.Educations.ToList());
        }

        public IActionResult Details(int id)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var education = _context.Educations
                .Include(e => e.Child)
                .ThenInclude(c => c.Parent)
                .ThenInclude(p => p.User)
                .Include(e => e.Instructor)
                .ThenInclude(i => i.User)
                .Include(e => e.Modules)
                .ThenInclude(m => m.Contents)
                .FirstOrDefault(e => e.Id == id && e.Instructor.UserId == userId);

            if (education == null)
            {
                return NotFound();
            }

            return View(education);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var instructor = _context.Instructors
                .Include(i => i.User)
                .FirstOrDefault(i => i.UserId == userId);

            if (instructor == null)
            {
                return NotFound();
            }

            ViewBag.Children = _context.Children
                .Include(c => c.Parent)
                .ThenInclude(p => p.User)
                .Where(c => c.IsActive)
                .ToList();

            return View(new Education { InstructorId = instructor.Id });
        }

        [HttpPost]
        public IActionResult Create(Education education)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Children = _context.Children
                    .Include(c => c.Parent)
                    .ThenInclude(p => p.User)
                    .Where(c => c.IsActive)
                    .ToList();

                return View(education);
            }

            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var instructor = _context.Instructors
                .FirstOrDefault(i => i.UserId == userId);

            if (instructor == null)
            {
                return NotFound();
            }

            education.InstructorId = instructor.Id;
            education.CreatedAt = DateTime.Now;
            education.IsActive = true;

            _context.Educations.Add(education);
            _context.SaveChanges();

            TempData["Success"] = "Eğitim başarıyla oluşturuldu.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var education = _context.Educations
                .Include(e => e.Child)
                .ThenInclude(c => c.Parent)
                .ThenInclude(p => p.User)
                .Include(e => e.Instructor)
                .ThenInclude(i => i.User)
                .FirstOrDefault(e => e.Id == id && e.Instructor.UserId == userId);

            if (education == null)
            {
                return NotFound();
            }

            ViewBag.Children = _context.Children
                .Include(c => c.Parent)
                .ThenInclude(p => p.User)
                .Where(c => c.IsActive)
                .ToList();

            return View(education);
        }

        [HttpPost]
        public IActionResult Edit(Education education)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Children = _context.Children
                    .Include(c => c.Parent)
                    .ThenInclude(p => p.User)
                    .Where(c => c.IsActive)
                    .ToList();

                return View(education);
            }

            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var existingEducation = _context.Educations
                .Include(e => e.Instructor)
                .FirstOrDefault(e => e.Id == education.Id && e.Instructor.UserId == userId);

            if (existingEducation == null)
            {
                return NotFound();
            }

            existingEducation.ChildId = education.ChildId;
            existingEducation.Title = education.Title;
            existingEducation.Description = education.Description;
            existingEducation.StartDate = education.StartDate;
            existingEducation.EndDate = education.EndDate;
            existingEducation.Goals = education.Goals;
            existingEducation.LastUpdatedAt = DateTime.Now;

            _context.SaveChanges();

            TempData["Success"] = "Eğitim başarıyla güncellendi.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var education = _context.Educations
                .Include(e => e.Instructor)
                .FirstOrDefault(e => e.Id == id && e.Instructor.UserId == userId);

            if (education == null)
            {
                return NotFound();
            }

            education.IsActive = false;
            education.LastUpdatedAt = DateTime.Now;

            _context.SaveChanges();

            TempData["Success"] = "Eğitim başarıyla silindi.";
            return RedirectToAction("Index");
        }
    }
} 
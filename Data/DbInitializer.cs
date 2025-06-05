using System;
using System.Linq;
using AutismEducationPlatform.Models;
using Microsoft.EntityFrameworkCore;

namespace AutismEducationPlatform.Data
{
    public static class DbInitializer
    {
        public static void Initialize(UygulamaDbContext context)
        {
            context.Database.EnsureCreated();

            // Admin kullanıcısı oluştur
            if (!context.Users.Any(u => u.IsAdmin))
            {
                var adminUser = new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@example.com",
                    Password = "Admin123!",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    IsActive = true,
                    IsAdmin = true,
                    UserType = UserType.Admin,
                    CreatedAt = DateTime.Now
                };

                context.Users.Add(adminUser);
                context.SaveChanges();

                var admin = new Admin
                {
                    UserId = adminUser.Id,
                    AccessLevel = 1,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                context.Admins.Add(admin);
                context.SaveChanges();
            }

            // Test velisi oluştur
            if (!context.Parents.Any())
            {
                var parentUser = new User
                {
                    FirstName = "Test",
                    LastName = "Parent",
                    Email = "parent@example.com",
                    Password = "Parent123!",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Parent123!"),
                    PhoneNumber = "5551234567",
                    Address = "Test Address",
                    IsActive = true,
                    UserType = UserType.Parent,
                    CreatedAt = DateTime.Now
                };

                context.Users.Add(parentUser);
                context.SaveChanges();

                var parent = new Parent
                {
                    UserId = parentUser.Id,
                    Occupation = "Test Occupation",
                    EmergencyContact = "5559876543",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                context.Parents.Add(parent);
                context.SaveChanges();
            }

            // Test eğitmeni oluştur
            if (!context.Instructors.Any())
            {
                var instructorUser = new User
                {
                    FirstName = "Test",
                    LastName = "Instructor",
                    Email = "instructor@example.com",
                    Password = "Instructor123!",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Instructor123!"),
                    PhoneNumber = "5557654321",
                    Address = "Test Address",
                    IsActive = true,
                    UserType = UserType.Instructor,
                    CreatedAt = DateTime.Now
                };

                context.Users.Add(instructorUser);
                context.SaveChanges();

                var instructor = new Instructor
                {
                    UserId = instructorUser.Id,
                    Specialization = "Test Specialization",
                    YearsOfExperience = 5,
                    Certifications = "Test Certifications",
                    EducationHistory = "Test Education History",
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                context.Instructors.Add(instructor);
                context.SaveChanges();
            }
        }
    }
} 
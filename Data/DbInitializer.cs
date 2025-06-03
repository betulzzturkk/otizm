using System;
using System.Linq;
using AutismEducationPlatform.Models;
using AutismEducationPlatform.Helpers;
using AutismEducationPlatform.Models.Enums;

namespace AutismEducationPlatform.Data
{
    public static class DbInitializer
    {
        public static void Initialize(UygulamaDbContext context)
        {
            context.Database.EnsureCreated();

            // Admin kullanıcısını kontrol et
            if (!context.Users.Any(u => u.IsAdmin))
            {
                var password = "admin1234";
                var hashedPassword = PasswordHasher.HashPassword(password);

                var adminUser = new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "betulgenc125@gmail.com",
                    Password = hashedPassword,
                    PasswordHash = hashedPassword,
                    UserType = UserType.Admin,
                    IsAdmin = true,
                    CreatedAt = DateTime.UtcNow,
                    IsInChildMode = false
                };

                context.Users.Add(adminUser);
                context.SaveChanges();
            }
        }
    }
} 
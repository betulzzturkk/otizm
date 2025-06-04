using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using AutismEducationPlatform.Models.Enums;

namespace AutismEducationPlatform.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsInChildMode { get; set; }

        public UserType UserType { get; set; } = UserType.Parent;

        public DateTime CreatedAt { get; set; }

        public ICollection<Child> Children { get; set; } = new List<Child>();
    }
} 
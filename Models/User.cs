using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AutismEducationPlatform.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsInChildMode { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Child> Children { get; set; }
    }
} 
using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class Student
    {
        public Student()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Parent = new User();
        }

        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        [Required]
        public User Parent { get; set; }
        public int ParentId { get; set; }

        // Track student's progress
        public int CurrentModuleId { get; set; }
        public DateTime LastActivityDate { get; set; }
    }
} 
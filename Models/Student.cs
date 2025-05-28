using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int ParentId { get; set; }  // Foreign key to User (Parent)

        public User Parent { get; set; }

        // Track student's progress
        public int CurrentModuleId { get; set; }
        public DateTime LastActivityDate { get; set; }
    }
} 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutismEducationPlatform.Models
{
    public class Child
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; } = string.Empty;

        public DateTime DiagnosisDate { get; set; }

        public string SpecialNotes { get; set; } = string.Empty;

        public string Medications { get; set; } = string.Empty;

        public string Allergies { get; set; } = string.Empty;

        public string EducationalHistory { get; set; } = string.Empty;

        public int ParentId { get; set; }

        [ForeignKey("ParentId")]
        public User Parent { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public ICollection<LearningProgress> LearningProgress { get; set; } = new List<LearningProgress>();
    }
} 
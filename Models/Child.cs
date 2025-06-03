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
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; }

        public DateTime DiagnosisDate { get; set; }

        public string SpecialNotes { get; set; }

        public string Medications { get; set; }

        public string Allergies { get; set; }

        public string EducationalHistory { get; set; }

        public int ParentId { get; set; }

        [ForeignKey("ParentId")]
        public User Parent { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<LearningProgress> LearningProgress { get; set; }
    }
} 
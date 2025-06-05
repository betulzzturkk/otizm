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

        [Required(ErrorMessage = "Ad alanı zorunludur")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad alanı zorunludur")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Doğum tarihi zorunludur")]
        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Cinsiyet")]
        public string? Gender { get; set; }

        [Display(Name = "Tanı")]
        public string? Diagnosis { get; set; }

        [Display(Name = "Tanı Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? DiagnosisDate { get; set; }

        [Display(Name = "Özel Notlar")]
        public string? SpecialNotes { get; set; }

        [Display(Name = "İlaçlar")]
        public string? Medications { get; set; }

        [Display(Name = "Alerjiler")]
        public string? Allergies { get; set; }

        [Display(Name = "Eğitim Geçmişi")]
        public string? EducationalHistory { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Kayıt Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Son Güncelleme")]
        public DateTime? LastUpdatedAt { get; set; }

        public int ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Parent Parent { get; set; } = null!;

        public ICollection<Education> Educations { get; set; } = new List<Education>();
        public ICollection<ProgressReport> ProgressReports { get; set; } = new List<ProgressReport>();
        public ICollection<ChildStatus> StatusHistory { get; set; } = new List<ChildStatus>();
        public ICollection<ParentInformation> ParentInformations { get; set; } = new List<ParentInformation>();
        public ICollection<LearningProgress> LearningProgresses { get; set; } = new List<LearningProgress>();
    }
} 
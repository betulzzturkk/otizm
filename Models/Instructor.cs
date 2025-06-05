using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutismEducationPlatform.Models
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Uzmanlık Alanı")]
        public string? Specialization { get; set; }

        [Display(Name = "Derece")]
        public string? Degree { get; set; }

        [Display(Name = "Sertifikalar")]
        public string? Certifications { get; set; }

        [Display(Name = "Deneyim (Yıl)")]
        public int? YearsOfExperience { get; set; }

        [Display(Name = "Eğitim Geçmişi")]
        public string? EducationHistory { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Kayıt Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Son Güncelleme")]
        public DateTime? LastUpdatedAt { get; set; }

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public ICollection<Education> Educations { get; set; } = new List<Education>();

        // User özelliklerini almak için
        [NotMapped]
        public string FirstName => User?.FirstName ?? string.Empty;

        [NotMapped]
        public string LastName => User?.LastName ?? string.Empty;

        [NotMapped]
        public string Email => User?.Email ?? string.Empty;

        [NotMapped]
        public string? PhoneNumber => User?.PhoneNumber;
    }
} 
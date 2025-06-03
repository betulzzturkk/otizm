using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models.ViewModels
{
    public class ChildInformationViewModel
    {
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

        [Required(ErrorMessage = "Cinsiyet seçimi zorunludur")]
        [Display(Name = "Cinsiyet")]
        public string Gender { get; set; } = string.Empty;

        [Display(Name = "Tanı Tarihi")]
        [DataType(DataType.Date)]
        public DateTime DiagnosisDate { get; set; }

        [Display(Name = "Özel Notlar")]
        public string? SpecialNotes { get; set; }

        [Display(Name = "İlaçlar")]
        public string? Medications { get; set; }

        [Display(Name = "Alerjiler")]
        public string? Allergies { get; set; }

        [Display(Name = "Eğitim Geçmişi")]
        public string? EducationalHistory { get; set; }
    }
} 
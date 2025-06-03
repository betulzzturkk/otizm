using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class ChildInformationViewModel
    {
        [Required(ErrorMessage = "Çocuğun adı zorunludur")]
        [Display(Name = "Çocuğun Adı")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Çocuğun soyadı zorunludur")]
        [Display(Name = "Çocuğun Soyadı")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Doğum tarihi zorunludur")]
        [DataType(DataType.Date)]
        [Display(Name = "Doğum Tarihi")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Cinsiyet seçimi zorunludur")]
        [Display(Name = "Cinsiyet")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Tanı tarihi zorunludur")]
        [DataType(DataType.Date)]
        [Display(Name = "Tanı Tarihi")]
        public DateTime DiagnosisDate { get; set; }

        [Display(Name = "Özel Notlar")]
        public string SpecialNotes { get; set; }

        [Display(Name = "Kullanılan İlaçlar")]
        public string Medications { get; set; }

        [Display(Name = "Alerjiler")]
        public string Allergies { get; set; }

        [Display(Name = "Eğitim Geçmişi")]
        public string EducationalHistory { get; set; }
    }
} 
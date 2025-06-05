using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class ChildStatus
    {
        [Key]
        public int Id { get; set; }

        public int ChildId { get; set; }
        public virtual Child Child { get; set; } = null!;

        [Required(ErrorMessage = "Durum açıklaması zorunludur")]
        [Display(Name = "Durum")]
        public string Status { get; set; } = string.Empty;

        [Display(Name = "Değerlendirme Tarihi")]
        public DateTime AssessmentDate { get; set; } = DateTime.Now;
    }
} 
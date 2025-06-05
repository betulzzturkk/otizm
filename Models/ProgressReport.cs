using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class ProgressReport
    {
        [Key]
        public int Id { get; set; }

        public int ChildId { get; set; }
        public virtual Child Child { get; set; } = null!;

        [Required(ErrorMessage = "Rapor açıklaması zorunludur")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Rapor Tarihi")]
        public DateTime ReportDate { get; set; } = DateTime.Now;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
} 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutismEducationPlatform.Models
{
    public class EducationModule
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur")]
        [Display(Name = "Ad")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Sıra No")]
        public int OrderNumber { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Kayıt Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Son Güncelleme")]
        public DateTime? LastUpdatedAt { get; set; }

        public int EducationId { get; set; }

        [ForeignKey("EducationId")]
        public Education Education { get; set; } = null!;

        // Navigation properties
        public ICollection<ModuleContent> Contents { get; set; } = new List<ModuleContent>();
    }
} 
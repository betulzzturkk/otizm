using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutismEducationPlatform.Models
{
    public class ModuleContent
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur")]
        [Display(Name = "Ad")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "İçerik Tipi")]
        public string ContentType { get; set; } = string.Empty;

        [Display(Name = "İçerik URL")]
        public string? ContentUrl { get; set; }

        [Display(Name = "Sıra No")]
        public int OrderNumber { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Kayıt Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Son Güncelleme")]
        public DateTime? LastUpdatedAt { get; set; }

        public int ModuleId { get; set; }

        [ForeignKey("ModuleId")]
        public EducationModule Module { get; set; } = null!;
    }
} 
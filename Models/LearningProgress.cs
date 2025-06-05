using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutismEducationPlatform.Models
{
    public class LearningProgress
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tamamlanan İçerik Sayısı")]
        public int CompletedContentCount { get; set; }

        [Display(Name = "Toplam İçerik Sayısı")]
        public int TotalContentCount { get; set; }

        [Display(Name = "İlerleme Yüzdesi")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ProgressPercentage { get; set; }

        [Display(Name = "Son Erişim")]
        public DateTime LastAccessDate { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Kayıt Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Son Güncelleme")]
        public DateTime? LastUpdatedAt { get; set; }

        public int ChildId { get; set; }

        [ForeignKey("ChildId")]
        public Child Child { get; set; } = null!;

        public int ModuleId { get; set; }

        [ForeignKey("ModuleId")]
        public LearningModule Module { get; set; } = null!;

        public string ModuleName { get; set; } = string.Empty;

        public string ItemName { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;
    }
} 
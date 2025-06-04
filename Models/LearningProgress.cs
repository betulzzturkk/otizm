using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutismEducationPlatform.Models
{
    public class LearningProgress
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ChildId { get; set; }

        [ForeignKey("ChildId")]
        public Child Child { get; set; } = null!;

        [Required]
        public int ModuleId { get; set; }

        [ForeignKey("ModuleId")]
        public LearningModule Module { get; set; } = null!;

        public string ModuleName { get; set; } = string.Empty;

        public string ItemName { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public int CompletedContentCount { get; set; }

        public int TotalContentCount { get; set; }

        public decimal ProgressPercentage { get; set; }

        public DateTime LastAccessDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
} 
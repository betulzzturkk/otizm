using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutismEducationPlatform.Models
{
    public class ModuleContent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ContentType { get; set; } = string.Empty;

        [Required]
        public string ContentUrl { get; set; } = string.Empty;

        public int OrderIndex { get; set; }

        public bool IsActive { get; set; }

        public int ModuleId { get; set; }

        [Required]
        public LearningModule Module { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
} 
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
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ContentType { get; set; }

        public string ContentUrl { get; set; }

        public int OrderIndex { get; set; }

        public bool IsActive { get; set; }

        public int ModuleId { get; set; }
        public LearningModule Module { get; set; }

        public DateTime CreatedAt { get; set; }
    }
} 
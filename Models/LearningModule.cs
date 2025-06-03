using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class LearningModule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int OrderIndex { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<ModuleContent> Contents { get; set; }

        public ICollection<LearningProgress> LearningProgress { get; set; }
    }
} 
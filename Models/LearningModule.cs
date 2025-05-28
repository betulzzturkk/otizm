using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class LearningModule
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ModuleType { get; set; }  // Colors, Animals, Shapes, TrafficSigns, Greetings

        public string ImagePath { get; set; }
        public string SoundPath { get; set; }

        public List<ModuleContent> Contents { get; set; }
    }

    public class ModuleContent
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string SoundPath { get; set; }
        
        public int LearningModuleId { get; set; }
        public LearningModule LearningModule { get; set; }
    }
} 
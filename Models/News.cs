using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        public string NewsUrl { get; set; } = string.Empty;

        public string Source { get; set; } = string.Empty;

        public DateTime PublishDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
} 
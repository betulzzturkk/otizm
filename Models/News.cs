using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public string NewsUrl { get; set; }

        public string Source { get; set; }

        public DateTime PublishDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }
    }
} 
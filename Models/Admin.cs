using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutismEducationPlatform.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Yetki Seviyesi")]
        public int AccessLevel { get; set; } = 1;

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Kayıt Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Son Güncelleme")]
        public DateTime? LastUpdatedAt { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        // User özelliklerini almak için
        [NotMapped]
        public string FirstName => User?.FirstName ?? string.Empty;

        [NotMapped]
        public string LastName => User?.LastName ?? string.Empty;

        [NotMapped]
        public string Email => User?.Email ?? string.Empty;

        [NotMapped]
        public string? PhoneNumber => User?.PhoneNumber;
    }
} 
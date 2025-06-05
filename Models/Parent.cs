using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutismEducationPlatform.Models
{
    public class Parent
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Meslek")]
        public string? Occupation { get; set; }

        [Display(Name = "Şehir")]
        public string? City { get; set; }

        [Display(Name = "Acil Durum İletişim")]
        public string? EmergencyContact { get; set; }

        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Kayıt Tarihi")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Son Güncelleme")]
        public DateTime? LastUpdatedAt { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        public ICollection<Child> Children { get; set; } = new List<Child>();

        // User özelliklerini almak için
        [NotMapped]
        public string FirstName => User?.FirstName ?? string.Empty;

        [NotMapped]
        public string LastName => User?.LastName ?? string.Empty;

        [NotMapped]
        public string Email => User?.Email ?? string.Empty;

        [NotMapped]
        public string? PhoneNumber => User?.PhoneNumber;

        [NotMapped]
        public string? Address => User?.Address;

        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
} 
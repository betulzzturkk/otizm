using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class CocukDurumu
    {
        public int Id { get; set; }

        public int CocukId { get; set; }
        public virtual User? Cocuk { get; set; }

        [Required(ErrorMessage = "Durum açıklaması zorunludur")]
        public string DurumAciklamasi { get; set; } = string.Empty;

        public DateTime Tarih { get; set; } = DateTime.UtcNow;
    }
} 
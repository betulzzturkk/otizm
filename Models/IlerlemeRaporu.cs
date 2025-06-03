using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class IlerlemeRaporu
    {
        public int Id { get; set; }

        public int OgrenciId { get; set; }
        public virtual User? Ogrenci { get; set; }

        [Required(ErrorMessage = "Rapor içeriği zorunludur")]
        public string Rapor { get; set; } = string.Empty;

        public DateTime Tarih { get; set; }
    }
} 
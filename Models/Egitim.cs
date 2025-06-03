using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class Egitim
    {
        public Egitim()
        {
            Baslik = string.Empty;
            Aciklama = string.Empty;
            OlusturulmaTarihi = DateTime.Now;
            IsActive = true;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Başlık alanı zorunludur")]
        [Display(Name = "Başlık")]
        public string Baslik { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur")]
        [Display(Name = "Açıklama")]
        public string Aciklama { get; set; }

        [Display(Name = "Video URL")]
        public string? VideoUrl { get; set; }

        [Display(Name = "Döküman URL")]
        public string? DokumanUrl { get; set; }

        [Display(Name = "Eğitim Süresi (Dakika)")]
        public int? EgitimSuresi { get; set; }

        [Display(Name = "Aktif Mi?")]
        public bool IsActive { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime OlusturulmaTarihi { get; set; }
    }
} 
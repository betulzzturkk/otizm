using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class Cocuk
    {
        public Cocuk()
        {
            Ad = string.Empty;
            Soyad = string.Empty;
            OzelDurum = string.Empty;
            EgitimDurumu = string.Empty;
            DogumTarihi = DateTime.Now;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur")]
        public string Soyad { get; set; }

        public int Yas { get; set; }
        public int VeliId { get; set; }
        public virtual Kullanici? Veli { get; set; }

        [Required(ErrorMessage = "Doğum tarihi zorunludur")]
        public DateTime DogumTarihi { get; set; }

        [Required(ErrorMessage = "Özel durum alanı zorunludur")]
        public string OzelDurum { get; set; }

        [Required(ErrorMessage = "Eğitim durumu zorunludur")]
        public string EgitimDurumu { get; set; }
    }
} 
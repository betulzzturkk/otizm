using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class Ogrenci
    {
        public Ogrenci()
        {
            Ad = string.Empty;
            Soyad = string.Empty;
            Cinsiyet = string.Empty;
            OzelDurum = string.Empty;
            IlgiAlanlari = string.Empty;
            Veli = new User();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur")]
        public string Soyad { get; set; }

        public DateTime DogumTarihi { get; set; }

        [Required(ErrorMessage = "Cinsiyet alanı zorunludur")]
        public string Cinsiyet { get; set; }

        public string OzelDurum { get; set; }

        public string IlgiAlanlari { get; set; }

        [Required]
        public User Veli { get; set; }
        public int VeliId { get; set; }

        // Öğrencinin ilerlemesini takip et
        public int GuncelModulId { get; set; }
        public DateTime SonAktiviteTarihi { get; set; }
    }
} 
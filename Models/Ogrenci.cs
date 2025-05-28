using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class Ogrenci
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı gereklidir")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı gereklidir")]
        public string Soyad { get; set; }

        [Required]
        public int VeliId { get; set; }  // Kullanici (Veli) tablosuna foreign key

        public Kullanici Veli { get; set; }

        // Öğrencinin ilerlemesini takip et
        public int GuncelModulId { get; set; }
        public DateTime SonAktiviteTarihi { get; set; }
    }
} 
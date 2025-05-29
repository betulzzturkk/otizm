using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class Kullanici
    {
        public Kullanici()
        {
            Ad = string.Empty;
            Soyad = string.Empty;
            Email = string.Empty;
            Sifre = string.Empty;
            KullaniciTipi = string.Empty;
            KullaniciAdi = string.Empty;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "E-posta alanı zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır")]
        public string Sifre { get; set; }

        [Required(ErrorMessage = "Kullanıcı tipi zorunludur")]
        public string KullaniciTipi { get; set; } // Admin, Veli, Egitmen
    }
} 
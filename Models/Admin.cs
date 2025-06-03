using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class Admin
    {
        public Admin()
        {
            KullaniciAdi = string.Empty;
            Sifre = string.Empty;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        public string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }
    }
} 
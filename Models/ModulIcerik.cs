using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class ModulIcerik
    {
        public ModulIcerik()
        {
            Baslik = string.Empty;
            Icerik = string.Empty;
            ResimYolu = string.Empty;
            SesYolu = string.Empty;
        }

        public int Id { get; set; }

        public int EgitimModuluId { get; set; }
        public virtual EgitimModulu? EgitimModulu { get; set; }

        [Required(ErrorMessage = "Başlık gereklidir")]
        public string Baslik { get; set; }

        [Required(ErrorMessage = "İçerik gereklidir")]
        public string Icerik { get; set; }

        public string ResimYolu { get; set; }
        public string SesYolu { get; set; }
        public int Sira { get; set; }
        public bool AktifMi { get; set; } = true;
    }
} 
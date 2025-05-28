using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class EgitimModulu
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Modül adı gereklidir")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Açıklama gereklidir")]
        public string Aciklama { get; set; }

        [Required(ErrorMessage = "Modül tipi gereklidir")]
        public string ModulTipi { get; set; }  // Renkler, Hayvanlar, Şekiller, TrafikLevhalari, Selamlaşma

        public string ResimYolu { get; set; }
        public string SesYolu { get; set; }

        public List<ModulIcerik> Icerikler { get; set; }
    }

    public class ModulIcerik
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Başlık gereklidir")]
        public string Baslik { get; set; }
        
        public string Aciklama { get; set; }
        public string ResimYolu { get; set; }
        public string SesYolu { get; set; }
        
        public int EgitimModuluId { get; set; }
        public EgitimModulu EgitimModulu { get; set; }
    }
} 
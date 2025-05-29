using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AutismEducationPlatform.Models
{
    public class EgitimModulu
    {
        public EgitimModulu()
        {
            ModulAdi = string.Empty;
            Aciklama = string.Empty;
            ModulTipi = string.Empty;
            ResimYolu = string.Empty;
            SesYolu = string.Empty;
            Icerikler = new List<ModulIcerik>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Modül adı gereklidir")]
        public string ModulAdi { get; set; }

        [Required(ErrorMessage = "Açıklama gereklidir")]
        public string Aciklama { get; set; }

        [Required(ErrorMessage = "Modül tipi gereklidir")]
        public string ModulTipi { get; set; }  // Renkler, Hayvanlar, Şekiller, vb.

        public string ResimYolu { get; set; }
        public string SesYolu { get; set; }

        public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;
        public bool AktifMi { get; set; } = true;

        // Navigation property
        public virtual ICollection<ModulIcerik> Icerikler { get; set; }
    }
} 
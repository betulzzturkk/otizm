using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class AdminPanelViewModel
    {
        public AdminPanelViewModel()
        {
            Veliler = new List<Kullanici>();
            Cocuklar = new List<Cocuk>();
            EgitimModulleri = new List<EgitimModulu>();
            CocukDurumlari = new List<CocukDurumu>();
        }

        public List<Kullanici> Veliler { get; set; }
        public List<Cocuk> Cocuklar { get; set; }
        public List<EgitimModulu> EgitimModulleri { get; set; }
        public List<CocukDurumu> CocukDurumlari { get; set; }
    }

    public class CocukDurumu
    {
        public CocukDurumu()
        {
            Durum = string.Empty;
            Aciklama = string.Empty;
            TarihSaat = DateTime.Now;
        }

        public int Id { get; set; }
        public int CocukId { get; set; }
        public virtual Cocuk? Cocuk { get; set; }

        [Required(ErrorMessage = "Durum alanı zorunludur")]
        public string Durum { get; set; }
        public DateTime TarihSaat { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur")]
        public string Aciklama { get; set; }
    }
} 
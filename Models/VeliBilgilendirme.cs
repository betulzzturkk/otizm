using System;
using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class VeliBilgilendirme
    {
        public VeliBilgilendirme()
        {
            Mesaj = string.Empty;
            TarihSaat = DateTime.Now;
            Okundu = false;
        }

        public int Id { get; set; }
        public int VeliId { get; set; }
        public virtual Kullanici? Veli { get; set; }

        [Required(ErrorMessage = "Mesaj alanı zorunludur")]
        public string Mesaj { get; set; }
        public DateTime TarihSaat { get; set; }
        public bool Okundu { get; set; }
    }
} 
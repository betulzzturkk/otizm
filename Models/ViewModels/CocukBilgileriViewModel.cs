using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models.ViewModels
{
    public class CocukBilgileriViewModel
    {
        public CocukBilgileriViewModel()
        {
            Ad = string.Empty;
            Soyad = string.Empty;
            Cinsiyet = string.Empty;
            OzelDurum = string.Empty;
            IlgiAlanlari = string.Empty;
        }

        [Required(ErrorMessage = "Ad alanı zorunludur")]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur")]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "Doğum tarihi zorunludur")]
        [DataType(DataType.Date)]
        public DateTime DogumTarihi { get; set; }

        [Required(ErrorMessage = "Cinsiyet alanı zorunludur")]
        public string Cinsiyet { get; set; }

        public string OzelDurum { get; set; }

        public string IlgiAlanlari { get; set; }
    }
} 
using AutismEducationPlatform.Models.Enums;

namespace AutismEducationPlatform.Models.ViewModels
{
    public class OgrenciViewModel
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public UserType KullaniciTipi { get; set; }
    }
} 
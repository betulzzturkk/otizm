using System.ComponentModel.DataAnnotations;

namespace AutismEducationPlatform.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Ad Soyad alanı zorunludur.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Konu alanı zorunludur.")]
        public string Subject { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mesaj alanı zorunludur.")]
        public string Message { get; set; } = string.Empty;
    }
} 
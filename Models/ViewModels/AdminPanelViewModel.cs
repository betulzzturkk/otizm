using System.Collections.Generic;

namespace AutismEducationPlatform.Models.ViewModels
{
    public class AdminPanelViewModel
    {
        public AdminPanelViewModel()
        {
            SonKayitOlanKullanicilar = new List<User>();
        }

        public int ToplamVeliSayisi { get; set; }
        public int ToplamEgitmenSayisi { get; set; }
        public ICollection<User> SonKayitOlanKullanicilar { get; set; }
    }
} 
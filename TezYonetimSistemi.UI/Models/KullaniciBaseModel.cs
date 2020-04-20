using System.ComponentModel.DataAnnotations;

namespace TezYonetimSistemi.UI.Models
{
    public abstract class KullaniciBaseModel
    {
        public int ID { get; set; }
        public int RolID { get; set; }

        [Required]
        [EmailAddress]
        public string Mail { get; set; }

        [Required]
        public string Sifre { get; set; }

        [Required]
        public string Ad { get; set; }

        [Required]
        public string Soyad { get; set; }

        public string Fotograf { get; set; }
    }
}
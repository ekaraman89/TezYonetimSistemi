using System.ComponentModel.DataAnnotations;

namespace TezYonetimSistemi.UI.Models
{
    public class OgretmenViewModel:KullaniciBaseModel
    {       
        [Required]
        public string Unvan { get; set; }
    }
}
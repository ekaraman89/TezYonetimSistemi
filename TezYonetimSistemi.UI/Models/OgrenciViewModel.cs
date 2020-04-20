using System.ComponentModel.DataAnnotations;

namespace TezYonetimSistemi.UI.Models
{
    public class OgrenciViewModel:KullaniciBaseModel
    {
        [Required]
        [RegularExpression("([0-9]+)",ErrorMessage ="Sadece Rakama girilebilir")]
        [MaxLength(20,ErrorMessage ="En Fazla 20 rakam girebilirsiniz")]
        public string OgrenciNo { get; set; }
    }
}
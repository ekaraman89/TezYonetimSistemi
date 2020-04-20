using System;
using System.ComponentModel.DataAnnotations;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.UI.Models
{
    public class TezDosyaYuklemeDuyurusuViewModel
    {
        public int ID { get; set; }

        [Required]
        public string Adi { get; set; }

        [Required]
        public DateTime SonDosyaYuklemeTarihi { get; set; }

        public bool Aktif { get; set; }

        [Required]
        public string Aciklama { get; set; }

        public TezDonemi TezDonemi { get; set; }

        public TezDersKod TezDersKod { get; set; }

    }
}
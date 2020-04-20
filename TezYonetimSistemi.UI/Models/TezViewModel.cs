using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TezYonetimSistemi.Model;
using TezYonetimSistemi.Services;

namespace TezYonetimSistemi.UI.Models
{
    public class TezViewModel
    {
        public TezViewModel()
        {

        }

        public TezViewModel(int ID)
        {

        }
        public int ID { get; set; }

        [Required]
        public string TezAdi { get; set; }

        public int OgretmenID { get; set; }

        public int TezKodID { get; set; }

        public string TezDonemi { get; set; }

        public int TezDonemID { get; set; }

        public string OgretimGorevlisi { get; set; }

        public DateTime EklenmeTarihi { get; set; }

        [Required(ErrorMessage = "Öğrenci seçin")]
        public int[] Ogrenciler { get; set; }

        private TezDersKod _tezDersKodu { get { return new TezDersKodService().TezDersKodlariGetir().SingleOrDefault(x => x.ID == this.TezKodID); } }

        public string TezDersKodu { get { return $"{_tezDersKodu.Kod} - {_tezDersKodu.Adi}"; } }

    }
}
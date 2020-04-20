using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TezYonetimSistemi.Model;
using TezYonetimSistemi.Services;

namespace TezYonetimSistemi.UI.Models
{
    public class OgrenciTezPartialVievModel
    {
        public OgrenciTezPartialVievModel(int tezID)
        {
            TezID = tezID;
        }

        private int TezID;

        private Tez _tez { get { return new TezService().TezleriGetir().SingleOrDefault(x => x.ID == TezID); } }
        private TezDersKod _tezDersKod { get { return new TezDersKodService().TezDersKodlariGetir().SingleOrDefault(x => x.ID == _tez.TezKodID); } }
        private TezDonemi _tezDonemi { get { return new TezDonemService().TezDonemiGetir().SingleOrDefault(x => x.ID == _tez.TezDonemID); } }
        private Ogretmen _ogretmen { get { return new OgretmenService().OgretmenleriGetir().SingleOrDefault(x => x.ID == _tez.OgretmenID); } }
        private List<Kullanici> _kullanicilar { get { return new KullaniciService().KullanicilariGetir().ToList(); } }


        public string TezAdi { get { return new TezService().TezleriGetir().SingleOrDefault(x => x.ID == TezID).TezAdi; } }
        public string TezDonemi { get { return $"{_tezDonemi.TezDonemAdi}"; } }
        public string TezDersKod { get { return $"{_tezDersKod.Kod} - {_tezDersKod.Adi}"; } }
        public Dictionary<int, string> Ogrenciler
        {
            get
            {
                Dictionary<int, string> ogrenciler = new Dictionary<int, string>();
                foreach (TezOgrenci item in _tezOgrenci)
                {
                    Kullanici kul = _kullanicilar.SingleOrDefault(x => x.ID == item.OgrenciID);
                    ogrenciler.Add(kul.ID, $"{kul.Ad} {kul.Soyad}");
                }
                return ogrenciler;
            }
        }


        private List<TezOgrenci> _tezOgrenci { get { return new TezOgrenciService().TezOgrencileriGetir().Where(x => x.TezID == _tez.ID).ToList(); } }

        public string Ogretmen
        {
            get
            {
                Kullanici kul = _kullanicilar.SingleOrDefault(x => x.ID == _tez.OgretmenID);
                return $"{_ogretmen.Unvan} {kul.Ad} {kul.Soyad}";
            }
        }

    }
}
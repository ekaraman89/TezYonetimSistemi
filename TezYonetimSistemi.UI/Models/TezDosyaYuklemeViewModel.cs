using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TezYonetimSistemi.Model;
using TezYonetimSistemi.Services;
using System.Linq;
using System;

namespace TezYonetimSistemi.UI.Models
{
    public class TezDosyaYuklemeViewModel
    {
        public int ID { get; set; }

        public Tez Tez { get; set; }

        public Kullanici Ogretmen { get; set; }

        public List<Kullanici> Ogrenci { get; set; }

        public TezDersKod TezDersKodu { get; set; }

        public TezDonemi TezDonemi { get; set; }

        public List<TezDosyaYukleme> TezDosyaYukleme { get; set; }

        public TezDosyaYuklemeDuyurusu TezDosyaYuklemeDuyurusu { get; set; }

        public int YuklenenOgrenciID { get; set; }

        public string DosyaAdi { get; set; }

        public string DosyaYolu { get; set; }

        [Required]
        public string DosyaAciklama { get; set; }

        public string Unvan { get; set; }

        public DateTime EklenmeTarihi { get; set; }

        public List<TezDosyaYuklemeViewModel> TezDosyaYuklemeListesi()
        {
            //Tez Adı - Ogretim Görevlisi - Tez Ders Kodu - Tez Donemi - Dosya Adı - Açıklaması - Yukleyen Ogrenci 

            List<Kullanici> _kullanicilar = new KullaniciService().KullanicilariGetir().ToList();

            List<TezDosyaYuklemeViewModel> lst = new List<TezDosyaYuklemeViewModel>();
            foreach (TezDosyaYukleme tezdosya in new TezDosyaYuklemeService().TezDosyaYuklemeGetir().ToList())
            {
                Tez _tez = new TezService().TezleriGetir().FirstOrDefault(x => x.ID == tezdosya.TezID);
                TezDersKod _tezDersKod = new TezDersKodService().TezDersKodlariGetir().FirstOrDefault(x => x.ID == _tez.TezKodID);
                TezDonemi _tezDonemi = new TezDonemService().TezDonemiGetir().FirstOrDefault(x => x.ID == _tez.TezDonemID);
                Ogretmen _ogretmen = new OgretmenService().OgretmenleriGetir().FirstOrDefault(x => x.ID == _tez.OgretmenID);
                TezDosyaYuklemeViewModel model = new TezDosyaYuklemeViewModel
                {
                    ID = tezdosya.ID,
                    DosyaAciklama = tezdosya.DosyaAciklama,
                    DosyaAdi = tezdosya.DosyaAdi,
                    DosyaYolu = tezdosya.DosyaYolu,
                    Ogrenci = _kullanicilar.Where(x => x.ID == tezdosya.YuklenenOgrenciID).ToList(),
                    Ogretmen = _kullanicilar.FirstOrDefault(x => x.ID == _tez.OgretmenID),
                    Tez = _tez,
                    TezDersKodu = _tezDersKod,
                    TezDonemi = _tezDonemi,
                    Unvan = _ogretmen.Unvan,
                    EklenmeTarihi = tezdosya.OlusturmaTarihi
                };
                lst.Add(model);
            }

            return lst;
        }
    }
}

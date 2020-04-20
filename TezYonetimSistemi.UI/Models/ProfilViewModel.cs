using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TezYonetimSistemi.Model;
using TezYonetimSistemi.Services;

namespace TezYonetimSistemi.UI.Models
{
    public class ProfilViewModel : KullaniciBaseModel
    {
        private List<Kullanici> _kullanicilar = new KullaniciService().KullanicilariGetir().ToList();

        public ProfilViewModel()
        {
            // boş kalacak
        }

        public ProfilViewModel(int KullaniciID)
        {
            Kullanici kul = _kullanicilar.SingleOrDefault(x => x.ID == KullaniciID);
            base.ID = kul.ID;
            base.Mail = kul.Mail;
            base.Ad = kul.Ad;
            base.Fotograf = kul.Fotograf;
            base.Soyad = kul.Soyad;
            base.Sifre = kul.Sifre;
            base.RolID = kul.RolID;
            if (kul.RolID == 2) // Ogretim Görevlisi
            {
                Unvan = new OgretmenService().OgretmenleriGetir().SingleOrDefault(x => x.ID == kul.ID).Unvan;
            }
            else if (kul.RolID == 3) // Ogrenci
            {
                OgrenciNo = new OgrenciService().OgrencileriGetir().SingleOrDefault(x => x.ID == kul.ID).OgrenciNumarasi;
            }

        }
        public string Unvan { get; set; }
        public string OgrenciNo { get; set; }

        [Required]
        public string MevcutSifre { get; set; }
        [Required]
        public string SifreTekrar { get; set; }


        internal void Guncelle()
        {

            Kullanici kullanici = new Kullanici
            {
                Ad = this.Ad,
                ID = this.ID,
                Fotograf = this.Fotograf,
                Mail = this.Mail,
                Soyad = this.Soyad,
                Sifre = this.Sifre,
                RolID = this.RolID
            };
            new KullaniciService().KullaniciGuncelle(kullanici);

            // Unvan ve Öğrenci numarasını kendisi değiştirmesin
            //if (RolID == 2) // Ogretim Görevlisi
            //{
            //    new OgretmenService().OgretmenGuncelle(new Ogretmen { ID = this.ID, Unvan = this.Unvan });
            //}
            //else if (RolID == 3) // Ogrenci
            //{
            //    new OgrenciService().OgrenciGuncelle(new Ogrenci { ID = this.ID, OgrenciNumarasi = this.OgrenciNo });
            //}
        }
    }
}
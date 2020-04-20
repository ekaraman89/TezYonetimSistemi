using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TezYonetimSistemi.Test
{
    class Program
    {
        static void Main(string[] args)
        {


            Console.ReadLine();
        }

        private static void Ogretmen()
        {
            using (var client = new OgretmenService.OgretmenServiceClient())
            {
                OgretmenService.Ogretmen ogretmen = new OgretmenService.Ogretmen
                {
                    ID = 8,
                    Unvan = "Yar.Doç"
                };

                //client.OgretmenSil(ogretmen);
                //client.OgretmenEkle(ogretmen);
                client.OgretmenGuncelle(ogretmen);
            }
        }

        private static void Ogrenci()
        {
            using (var client = new OgrenciService.OgrenciServiceClient())
            {
                OgrenciService.Ogrenci ogr = new OgrenciService.Ogrenci
                {
                    ID = 8,
                    OgrenciNumarasi = "123"
                };
                //client.OgrenciSil(ogr);
                //client.OgrenciEkle(ogr);
                client.OgrenciGuncelle(ogr);
            }
        }

        private static void Kullanici()
        {
            using (var client = new KullaniciService.KullaniciServiceClient())
            {

                KullaniciService.Kullanici kul = new KullaniciService.Kullanici
                {
                    ID = 12,
                    Mail = "mail guncel",
                    Sifre = "sifre guncel",
                    RolID = 2,
                    Ad = "ad guncel",
                    Soyad = "soyad guncel",
                    Fotograf = "foto"
                };

                //client.KullaniciEkle(kul);
                //client.KullaniciGuncelle(kul);
                //client.KullaniciSil(kul);


            }
        }
    }
}

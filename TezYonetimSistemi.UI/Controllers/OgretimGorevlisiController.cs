using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TezYonetimSistemi.Model;
using TezYonetimSistemi.Services;
using TezYonetimSistemi.UI.Filters;
using TezYonetimSistemi.UI.Models;

namespace TezYonetimSistemi.UI.Controllers
{
    [CustomAuth]
    [OgretmenRole]
    public class OgretimGorevlisiController : Controller
    {

        private int OgretimGorevlisiID;
        public OgretimGorevlisiController()
        {
            if ((Kullanici)Helpers.CacheProvider.CachedenOku("kullanici") != null)
            {
                OgretimGorevlisiID = ((Kullanici)Helpers.CacheProvider.CachedenOku("kullanici")).ID;
            }
        }

        public ActionResult Detay(int? ID)
        {
            if (ID != null)
            {
                Ogretmen ogr = new OgretmenService().OgretmenleriGetir().SingleOrDefault(x => x.ID == ID);
                Kullanici kul = new KullaniciService().KullanicilariGetir().SingleOrDefault(x => x.ID == ID);
                if (kul != null && ogr != null)
                {
                    OgretmenViewModel model = new OgretmenViewModel
                    {
                        ID = ogr.ID,
                        Ad = kul.Ad,
                        Fotograf = kul.Fotograf,
                        Mail = kul.Mail,
                        Unvan = ogr.Unvan,
                        Soyad = kul.Soyad
                    };
                    ViewBag.OgretimGorevlisininTezleri = OgretimGorevlisininTezleri(ID);
                    return View(model);
                }
            }
            ViewBag.Mesaj = "Kullanıcı bulunamadı...";
            return null;
        }
        public ActionResult Index()
        {
            if (OgretimGorevlisiID == 0)
            {
                OgretimGorevlisiID = ((Kullanici)Session["kullanici"]).ID;
            }
            List<TezViewModel> lst = TezGetir().Where(x => x.OgretmenID == OgretimGorevlisiID).OrderByDescending(x => x.EklenmeTarihi).Take(10).ToList();
            ViewBag.lst = lst;
            ViewBag.OgretimGorevlisiID = OgretimGorevlisiID;
            return View();
        }

        public ActionResult Tezler()
        {

            return View(OgretimGorevlisininTezleri());
        }

        private List<TezViewModel> OgretimGorevlisininTezleri(int? ogretimGorevlisiID = null)
        {
            if (ogretimGorevlisiID != null)
            {
                OgretimGorevlisiID = (int)ogretimGorevlisiID;
            }
            List<TezViewModel> lst = new List<TezViewModel>();

            TezService client = new TezService();

            List<Kullanici> kullanicilar = new KullaniciService().KullanicilariGetir().ToList();
            foreach (Tez item in client.TezleriGetir().Where(x => x.OgretmenID == OgretimGorevlisiID))
            {
                TezViewModel tvm = new TezViewModel
                {
                    ID = item.ID,
                    TezAdi = item.TezAdi,
                    TezDonemID = item.TezDonemID,
                    TezKodID = item.TezKodID,
                    TezDonemi = new TezDonemService().TezDonemiGetir().Where(x => x.ID == item.TezDonemID).SingleOrDefault().TezDonemAdi
                };
                lst.Add(tvm);
            }

            return lst;
        }

        public ActionResult TezDuzenle(int ID)
        {
            Tez tez = new TezService().TezleriGetir().Where(x => x.ID == ID).SingleOrDefault();
            if (tez != null)
            {
                TezViewModel tvm = new TezViewModel
                {
                    ID = tez.ID,
                    OgretmenID = OgretimGorevlisiID,
                    TezAdi = tez.TezAdi,
                    TezDonemID = tez.TezDonemID,
                    TezKodID = tez.TezKodID,
                    Ogrenciler = new TezOgrenciService().TezOgrencileriGetir().Where(x => x.TezID == tez.ID).Select(x => x.OgrenciID).ToArray()
                };


                TempData["TezDonemi"] = new TezDonemService().TezDonemiGetir().Last();
                ViewBag.TezDonemi = TempData["TezDonemi"];

                TempData["Ogrenciler"] = TezAlabilenOgrenciler(tvm.Ogrenciler);
                ViewBag.Ogrenciler = TempData["Ogrenciler"];

                TempData["TezDersKodu"] = new TezDersKodService().TezDersKodlariGetir();
                ViewBag.TezDersKodu = TempData["TezDersKodu"];

                return View(tvm);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult TezDuzenle(TezViewModel model)
        {
            if (!ModelState.IsValid)
            {

                TempData["TezDonemi"] = new TezDonemService().TezDonemiGetir().Last();
                ViewBag.TezDonemi = TempData["TezDonemi"];

                TempData["Ogrenciler"] = TezAlabilenOgrenciler();
                ViewBag.Ogrenciler = TempData["Ogrenciler"];

                TempData["TezDersKodu"] = new TezDersKodService().TezDersKodlariGetir();
                ViewBag.TezDersKodu = TempData["TezDersKodu"];
                model.Ogrenciler = new TezOgrenciService().TezOgrencileriGetir().Where(x => x.TezID == model.ID).Select(x => x.OgrenciID).ToArray();
                return View(model);
            }

            TezService client = new TezService();
            Tez tez = client.TezleriGetir().SingleOrDefault(x => x.ID == model.ID);
            if (tez != null)
            {
                tez.TezAdi = model.TezAdi;
                tez.TezDonemID = model.TezDonemID;
                tez.OgretmenID = OgretimGorevlisiID;
                tez.TezKodID = model.TezKodID;

                client.TezGuncelle(tez);


                TezOgrenciService tezOgrencileri = new TezOgrenciService();
                foreach (var item in tezOgrencileri.TezOgrencileriGetir().Where(x => x.TezID == tez.ID))
                {
                    tezOgrencileri.TezOgrenciSil(item);
                }

                foreach (int i in model.Ogrenciler)
                {
                    new TezOgrenciService().TezOgrenciEkle(new TezOgrenci { TezID = tez.ID, OgrenciID = i });
                }

                TempData["TezDonemi"] = new TezDonemService().TezDonemiGetir().Last();
                ViewBag.TezDonemi = TempData["TezDonemi"];


                TempData["Ogrenciler"] = TezAlabilenOgrenciler();
                ViewBag.Ogrenciler = TempData["Ogrenciler"];

                TempData["TezDersKodu"] = new TezDersKodService().TezDersKodlariGetir();
                ViewBag.TezDersKodu = TempData["TezDersKodu"];
                ViewBag.Mesaj = $"<div class='alert alert-success'><strong>Başarılı!</strong> Tez Başarıyla Güncellendi... </div>";

                return View(model);
            }
            return View();
        }

        public ActionResult TezDetay(int ID)
        {
            ViewBag.ID = ID;
            return View();
        }


        private static List<OgrenciViewModel> TezAlabilenOgrenciler()
        {
            int tezDonemID = new TezDonemService().TezDonemiGetir().Last().ID;
            int[] DonemdekiTezler = new TezService().TezleriGetir().Where(x => x.TezDonemID == tezDonemID).Select(x => x.ID).ToArray();
            int[] DonemdekiOgrenciler = new TezOgrenciService().TezOgrencileriGetir().Where(x => DonemdekiTezler.Contains(x.TezID)).Select(x => x.OgrenciID).ToArray();


            List<OgrenciViewModel> lst = new List<OgrenciViewModel>();
            List<Kullanici> kullanicilar = new KullaniciService().KullanicilariGetir().ToList();
            List<Ogrenci> ogrenciler = new OgrenciService().OgrencileriGetir().ToList().Where(x => !DonemdekiOgrenciler.Contains(x.ID)).ToList();

            foreach (var item in ogrenciler)
            {
                Kullanici _kul = kullanicilar.Where(x => x.ID == item.ID).SingleOrDefault();
                OgrenciViewModel ovm = new OgrenciViewModel
                {
                    ID = item.ID,
                    OgrenciNo = item.OgrenciNumarasi,
                    Ad = _kul.Ad,
                    Fotograf = _kul.Fotograf,
                    Mail = _kul.Mail,
                    Soyad = _kul.Soyad
                };
                lst.Add(ovm);
            }
            return lst;
        }

        private static List<OgrenciViewModel> TezAlabilenOgrenciler(int[] TezAlmisOgrenciler)
        {
            List<OgrenciViewModel> lst = TezAlabilenOgrenciler();

            lst.AddRange(OgrencileriGetir().Where(x => TezAlmisOgrenciler.Contains(x.ID)));
            return lst;
        }

        private static List<OgrenciViewModel> OgrencileriGetir()
        {
            List<OgrenciViewModel> lst = new List<OgrenciViewModel>();
            List<Kullanici> kullanicilar = new KullaniciService().KullanicilariGetir().ToList();
            List<Ogrenci> ogrenciler = new OgrenciService().OgrencileriGetir().ToList();

            foreach (var item in ogrenciler)
            {
                Kullanici _kul = kullanicilar.Where(x => x.ID == item.ID).SingleOrDefault();
                OgrenciViewModel ovm = new OgrenciViewModel
                {
                    ID = item.ID,
                    OgrenciNo = item.OgrenciNumarasi,
                    Ad = _kul.Ad,
                    Fotograf = _kul.Fotograf,
                    Mail = _kul.Mail,
                    Soyad = _kul.Soyad
                };
                lst.Add(ovm);
            }

            return lst;
        }

        private static List<TezViewModel> TezGetir()
        {
            List<TezViewModel> lst = new List<TezViewModel>();

            TezService client = new TezService();


            List<Kullanici> kullanicilar = new KullaniciService().KullanicilariGetir().ToList();
            List<Ogretmen> ogretmenler = new OgretmenService().OgretmenleriGetir().ToList();
            foreach (Tez item in client.TezleriGetir())
            {
                TezViewModel tvm = new TezViewModel
                {
                    ID = item.ID,
                    TezAdi = item.TezAdi,
                    TezDonemID = item.TezDonemID,
                    TezKodID = item.TezKodID,
                    OgretmenID = item.OgretmenID,
                    TezDonemi = new TezDonemService().TezDonemiGetir().Where(x => x.ID == item.TezDonemID).SingleOrDefault().TezDonemAdi,
                    EklenmeTarihi = item.OlusturmaTarihi
                };
                OgretmenViewModel tmp = OgretmenleriGetir().SingleOrDefault(x => x.ID == item.OgretmenID);
                tvm.OgretimGorevlisi = $"{tmp.Unvan} {tmp.Ad} {tmp.Soyad}";
                lst.Add(tvm);
            }

            return lst;
        }

        private static List<OgretmenViewModel> OgretmenleriGetir()
        {
            List<OgretmenViewModel> lst = new List<OgretmenViewModel>();
            List<Kullanici> kullanicilar = new KullaniciService().KullanicilariGetir().ToList();
            List<Ogretmen> ogretmenler = new OgretmenService().OgretmenleriGetir().ToList();

            foreach (var item in ogretmenler)
            {
                Kullanici _kul = kullanicilar.Where(x => x.ID == item.ID).SingleOrDefault();
                OgretmenViewModel ovm = new OgretmenViewModel
                {
                    ID = item.ID,
                    Unvan = item.Unvan,
                    Ad = _kul.Ad,
                    Fotograf = _kul.Fotograf,
                    Mail = _kul.Mail,
                    Soyad = _kul.Soyad
                };
                lst.Add(ovm);
            }

            return lst;
        }
    }
}
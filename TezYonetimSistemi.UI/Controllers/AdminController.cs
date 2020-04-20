using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TezYonetimSistemi.Model;
using TezYonetimSistemi.Services;
using TezYonetimSistemi.UI.Filters;
using TezYonetimSistemi.UI.Helpers;
using TezYonetimSistemi.UI.Models;

namespace TezYonetimSistemi.UI.Controllers
{

    [CustomAuth]
    [AdminRole]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            // Son eklenen 10 tez
            ViewBag.lst = TezGetir().OrderByDescending(x => x.EklenmeTarihi).Take(10).ToList();

            return View();
        }

        #region Öğretmen
        public ActionResult Ogretmenler()
        {
            List<OgretmenViewModel> lst = OgretmenleriGetir();
            return View(lst);
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

        public ActionResult OgretmenEKle()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OgretmenEkle(OgretmenViewModel model, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                model.Fotograf = FileUpload.FileName(image, FileUpload.UploadFolder.Profil);
                model.RolID = 2;
                if (KullaniciKontrol(model.Mail))
                {
                    OgretmenEkle(KullaniciEkle(model), model.Unvan);
                }
                else
                {
                    ViewBag.Mesaj = $"<div class='alert alert-danger'><strong>Hata!</strong> Mail adresi kullanılıyor... </div>";
                    return View();
                }

                ViewBag.Mesaj = $"<div class='alert alert-success'><strong>Başarılı!</strong> Öğretim Görevlisi Başarıyla Eklendi... </div>";
                return View();
            }

            return View();
        }

        public ActionResult OgretmenDuzenle(int ID)
        {
            OgretmenViewModel ovm = OgretmenleriGetir().Where(x => x.ID == ID).SingleOrDefault();
            if (ovm != null)
            {
                return View(ovm);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult OgretmenDuzenle(OgretmenViewModel model, HttpPostedFileBase image)
        {
            ModelState.Remove("Sifre");
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    model.Fotograf = FileUpload.FileName(image, FileUpload.UploadFolder.Profil);
                }
                if (KullaniciKontrol(model.Mail, model.ID))
                {
                    if (string.IsNullOrWhiteSpace(model.Sifre)) model.Sifre = new KullaniciService().KullanicilariGetir().SingleOrDefault(x => x.ID == model.ID).Sifre;
                    KullaniciGuncelle(model);
                    OgretmenGuncelle(model.ID, model.Unvan);
                    model.Sifre = string.Empty;
                }
                else
                {

                    ViewBag.Mesaj = $"<div class='alert alert-danger'><strong>Hata!</strong> Mail adresi kullanılıyor... </div>";
                    return View(model);
                }

                ViewBag.Mesaj = $"<div class='alert alert-success'><strong>Başarılı!</strong> Öğretim Görevlisi Başarıyla Güncellendi... </div>";
                return View(model);
            }
            return View(model);
        }


        public string OgretmenSil(int ID)
        {
            var mesaj = "";
            OgretmenService client = new OgretmenService();
            Ogretmen ogr = client.OgretmenleriGetir().Where(x => x.ID == ID).SingleOrDefault();
            if (ogr != null)
            {
                if (new TezService().TezleriGetir().Where(x => x.OgretmenID == ogr.ID).FirstOrDefault() == null)
                {
                    client.OgretmenSil(ogr);
                    new KullaniciService().KullaniciSil(new Kullanici { ID = ogr.ID });
                    mesaj = JsonConvert.SerializeObject(new { durum = "OK", mesaj = "Ogretmen Silindi" });
                }
                else
                {
                    mesaj = JsonConvert.SerializeObject(new { durum = "NO", mesaj = "Tez atanmış olan öğretmen silinemez" });
                }
            }
            return mesaj;
        }

        #endregion

        #region Öğrenci
        public ActionResult Ogrenciler()
        {
            List<OgrenciViewModel> lst = OgrencileriGetir();
            return View(lst);
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

        public ActionResult OgrenciEkle()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OgrenciEkle(OgrenciViewModel model, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                model.Fotograf = FileUpload.FileName(image, FileUpload.UploadFolder.Profil);
                model.RolID = 3;
                if (KullaniciKontrol(model.Mail))
                {
                    if (OgrenciNoKontrol(model.OgrenciNo))
                    {
                        OgrenciEkle(KullaniciEkle(model), model.OgrenciNo);
                    }
                    else
                    {
                        ViewBag.Mesaj = $"<div class='alert alert-danger'><strong>Hata!</strong> Öğrenci Numarası zaten kullanılıyor... </div>";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Mesaj = $"<div class='alert alert-danger'><strong>Hata!</strong> Mail adresi kullanılıyor... </div>";
                    return View();
                }

                ViewBag.Mesaj = $"<div class='alert alert-success'><strong>Başarılı!</strong> Öğrenci Başarıyla Eklendi... </div>";
                return View();
            }
            return View();
        }

        public ActionResult OgrenciDuzenle(int ID)
        {
            OgrenciViewModel ovm = OgrencileriGetir().Where(x => x.ID == ID).SingleOrDefault();
            if (ovm != null)
            {
                ovm.Sifre = string.Empty;
                return View(ovm);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult OgrenciDuzenle(OgrenciViewModel model, HttpPostedFileBase image)
        {
            ModelState.Remove("Sifre");
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    model.Fotograf = FileUpload.FileName(image, FileUpload.UploadFolder.Profil);
                }
                if (KullaniciKontrol(model.Mail, model.ID))
                {
                    if (OgrenciNoKontrol(model.OgrenciNo))
                    {
                        if (string.IsNullOrWhiteSpace(model.Sifre)) model.Sifre = new KullaniciService().KullanicilariGetir().SingleOrDefault(x => x.ID == model.ID).Sifre;
                        KullaniciGuncelle(model);
                        OgrenciGuncelle(model.ID, model.OgrenciNo);
                        model.Sifre = string.Empty;
                    }
                    else
                    {
                        ViewBag.Mesaj = $"<div class='alert alert-danger'><strong>Hata!</strong> Öğrenci Numarası zaten kullanılıyor... </div>";
                        return View(model);
                    }
                }
                else
                {

                    ViewBag.Mesaj = $"<div class='alert alert-danger'><strong>Hata!</strong> Mail adresi kullanılıyor... </div>";
                    return View(model);
                }

                ViewBag.Mesaj = $"<div class='alert alert-success'><strong>Başarılı!</strong> Öğrenci Başarıyla Güncellendi... </div>";
                return View(model);
            }
            return View(model);

        }

        public string OgrenciSil(int ID)
        {
            var mesaj = "";
            OgrenciService client = new OgrenciService();
            Ogrenci ogr = client.OgrencileriGetir().Where(x => x.ID == ID).SingleOrDefault();
            if (ogr != null)
            {
                if (new TezOgrenciService().TezOgrencileriGetir().Where(x => x.OgrenciID == ogr.ID).FirstOrDefault() == null)
                {
                    client.OgrenciSil(ogr);
                    new KullaniciService().KullaniciSil(new Kullanici { ID = ogr.ID });
                    mesaj = JsonConvert.SerializeObject(new { durum = "OK", mesaj = "Ogrenci Silindi" });
                }
                else
                {
                    mesaj = JsonConvert.SerializeObject(new { durum = "NO", mesaj = "Tez atanmış olan öğrenci silinemez" });
                }
            }
            return mesaj;
        }


        #endregion

        #region Tez
        public ActionResult Tezler()
        {
            List<TezViewModel> lst = TezGetir();

            return View(lst);
        }



        public ActionResult TezEkle()
        {
            TempData["TezDonemi"] = new TezDonemService().TezDonemiGetir().Last();
            ViewBag.TezDonemi = TempData["TezDonemi"];

            TempData["Ogretmenler"] = OgretmenleriGetir();
            ViewBag.Ogretmenler = TempData["Ogretmenler"];

            TempData["Ogrenciler"] = TezAlabilenOgrenciler();
            ViewBag.Ogrenciler = TempData["Ogrenciler"];

            TempData["TezDersKodu"] = new TezDersKodService().TezDersKodlariGetir();
            ViewBag.TezDersKodu = TempData["TezDersKodu"];
            return View();
        }

        [HttpPost]
        public ActionResult TezEkle(TezViewModel model)
        {
            if (!ModelState.IsValid)
            {

                TempData["TezDonemi"] = new TezDonemService().TezDonemiGetir().Last();
                ViewBag.TezDonemi = TempData["TezDonemi"];

                TempData["Ogretmenler"] = OgretmenleriGetir();
                ViewBag.Ogretmenler = TempData["Ogretmenler"];

                TempData["Ogrenciler"] = TezAlabilenOgrenciler();
                ViewBag.Ogrenciler = TempData["Ogrenciler"];

                TempData["TezDersKodu"] = new TezDersKodService().TezDersKodlariGetir();
                ViewBag.TezDersKodu = TempData["TezDersKodu"];

                return View(model);
            }

            TezService client = new TezService();
            Tez tez = new Tez
            {
                TezAdi = model.TezAdi,
                TezDonemID = model.TezDonemID,
                OgretmenID = model.OgretmenID,
                TezKodID = model.TezKodID,
            };

            int tezID = client.TezEkle(tez).ID;

            foreach (int i in model.Ogrenciler)
            {
                new TezOgrenciService().TezOgrenciEkle(new TezOgrenci { TezID = tezID, OgrenciID = i });
            }

            ViewBag.Mesaj = $"<div class='alert alert-success'><strong>Başarılı!</strong> Tez Başarıyla Eklendi... </div>";

            TempData["TezDonemi"] = new TezDonemService().TezDonemiGetir().Last();
            ViewBag.TezDonemi = TempData["TezDonemi"];

            TempData["Ogretmenler"] = OgretmenleriGetir();
            ViewBag.Ogretmenler = TempData["Ogretmenler"];

            TempData["Ogrenciler"] = TezAlabilenOgrenciler();
            ViewBag.Ogrenciler = TempData["Ogrenciler"];

            TempData["TezDersKodu"] = new TezDersKodService().TezDersKodlariGetir();
            ViewBag.TezDersKodu = TempData["TezDersKodu"];
            return View();
            //return RedirectToAction("TezEkle");
        }

        public ActionResult TezDuzenle(int ID)
        {
            Tez tez = new TezService().TezleriGetir().Where(x => x.ID == ID).SingleOrDefault();
            if (tez != null)
            {
                TezViewModel tvm = new TezViewModel
                {
                    ID = tez.ID,
                    OgretmenID = tez.OgretmenID,
                    TezAdi = tez.TezAdi,
                    TezDonemID = tez.TezDonemID,
                    TezKodID = tez.TezKodID,
                    Ogrenciler = new TezOgrenciService().TezOgrencileriGetir().Where(x => x.TezID == tez.ID).Select(x => x.OgrenciID).ToArray()
                };


                TempData["TezDonemi"] = new TezDonemService().TezDonemiGetir().Last();
                ViewBag.TezDonemi = TempData["TezDonemi"];

                TempData["Ogretmenler"] = OgretmenleriGetir();
                ViewBag.Ogretmenler = TempData["Ogretmenler"];

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

                TempData["Ogretmenler"] = OgretmenleriGetir();
                ViewBag.Ogretmenler = TempData["Ogretmenler"];

                TempData["Ogrenciler"] = TezAlabilenOgrenciler();
                ViewBag.Ogrenciler = TempData["Ogrenciler"];

                TempData["TezDersKodu"] = new TezDersKodService().TezDersKodlariGetir();
                ViewBag.TezDersKodu = TempData["TezDersKodu"];

                return View(model);
            }

            TezService client = new TezService();
            Tez tez = client.TezleriGetir().SingleOrDefault(x => x.ID == model.ID);
            if (tez != null)
            {
                tez.TezAdi = model.TezAdi;
                tez.TezDonemID = model.TezDonemID;
                tez.OgretmenID = model.OgretmenID;
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

                TempData["Ogretmenler"] = OgretmenleriGetir();
                ViewBag.Ogretmenler = TempData["Ogretmenler"];

                TempData["Ogrenciler"] = TezAlabilenOgrenciler();
                ViewBag.Ogrenciler = TempData["Ogrenciler"];

                TempData["TezDersKodu"] = new TezDersKodService().TezDersKodlariGetir();
                ViewBag.TezDersKodu = TempData["TezDersKodu"];
                ViewBag.Mesaj = $"<div class='alert alert-success'><strong>Başarılı!</strong> Tez Başarıyla Güncellendi... </div>";

                return View(model);
            }
            return View();
        }

        public string TezSil(int ID)
        {
            var mesaj = "";
            TezService client = new TezService();
            Tez tez = client.TezleriGetir().Where(x => x.ID == ID).SingleOrDefault();
            if (tez != null)
            {
                //List<TezOgrenci> lst = ;
                foreach (TezOgrenci item in new TezOgrenciService().TezOgrencileriGetir().Where(x => x.TezID == tez.ID).ToList())
                {
                    new TezOgrenciService().TezOgrenciSil(item);
                }
                foreach (TezDosyaYukleme item in new TezDosyaYuklemeService().TezDosyaYuklemeGetir().Where(x => x.TezID == tez.ID))
                {
                    new TezDosyaYuklemeService().TezDosyaYuklemeSil(item);
                }
                client.TezSil(tez);

                mesaj = JsonConvert.SerializeObject(new { durum = "OK", mesaj = "Tez Silindi" });
            }
            return mesaj;
        }


        public ActionResult TezDetay(int ID)
        {
            ViewBag.ID = ID;
            return View();
        }
        #endregion

        #region Tez Dosya Yükleme Duyurusu

        public ActionResult TezDosyaYuklemeDuyurulari()
        {

            List<TezDosyaYuklemeDuyurusuViewModel> lst = new List<TezDosyaYuklemeDuyurusuViewModel>();

            foreach (TezDosyaYuklemeDuyurusu item in new TezDosyaYuklemeDuyuruService().TezDosyaYuklemeDuyurulariGetir().ToList())
            {
                TezDosyaYuklemeDuyurusuViewModel _tdydvm = new TezDosyaYuklemeDuyurusuViewModel
                {
                    ID = item.ID,
                    Aciklama = item.Aciklama,
                    Adi = item.Adi,
                    Aktif = item.Aktif,
                    TezDonemi = new TezDonemService().TezDonemiGetir().Where(x => x.ID == item.TezDonemID).SingleOrDefault(),
                    TezDersKod = new TezDersKodService().TezDersKodlariGetir().Where(x => x.ID == item.DersKoduID).SingleOrDefault()
                };
                lst.Add(_tdydvm);
            }

            return View(lst);
        }

        public ActionResult TezDosyaYuklemeDuyurusuEkle()
        {
            TempData["TezDonemi"] = new TezDonemService().TezDonemiGetir().Last();
            ViewBag.TezDonemi = TempData["TezDonemi"];

            TempData["TezDersKodu"] = new TezDersKodService().TezDersKodlariGetir();
            ViewBag.TezDersKodu = TempData["TezDersKodu"];
            return View();
        }

        [HttpPost]
        public ActionResult TezDosyaYuklemeDuyurusuEkle(TezDosyaYuklemeDuyurusuViewModel model, List<HttpPostedFileBase> files)
        {
            if (!ModelState.IsValid)
            {

                TempData["TezDonemi"] = new TezDonemService().TezDonemiGetir().Last();
                ViewBag.TezDonemi = TempData["TezDonemi"];

                TempData["TezDersKodu"] = new TezDersKodService().TezDersKodlariGetir();
                ViewBag.TezDersKodu = TempData["TezDersKodu"];

                return View(model);
            }

            TezDosyaYuklemeDuyuruService client = new TezDosyaYuklemeDuyuruService();

            TezDosyaYuklemeDuyurusu duyuru = new TezDosyaYuklemeDuyurusu
            {
                Adi = model.Adi,
                Aciklama = model.Aciklama,
                Aktif = true,
                DersKoduID = model.TezDersKod.ID,
                SonDosyaYuklemeTarihi = model.SonDosyaYuklemeTarihi,
                TezDonemID = model.TezDonemi.ID
            };


            TezDosyaYuklemeDuyurusuDosyasiEkle(files, client.TezDosyaYuklemeDuyurusuEkle(duyuru).ID);

            ViewBag.Mesaj = $"<div class='alert alert-success'><strong>Başarılı!</strong> Tez Dosya Duyurusu Başarıyla Eklendi... </div>";

            TempData["TezDonemi"] = new TezDonemService().TezDonemiGetir().Last();
            ViewBag.TezDonemi = TempData["TezDonemi"];

            TempData["TezDersKodu"] = new TezDersKodService().TezDersKodlariGetir();
            ViewBag.TezDersKodu = TempData["TezDersKodu"];

            return View();
        }

        private static void TezDosyaYuklemeDuyurusuDosyasiEkle(List<HttpPostedFileBase> files, int tezDosyaYuklemeDuyurusuID)
        {
            if (files.Count() > 0 && files[0] != null)
            {
                foreach (var file in files)
                {
                    string dosyaAdi = Helpers.FileUpload.FileName(file, Helpers.FileUpload.UploadFolder.DuyuruYuklemeDosyasi);
                    TezDosyaYuklemeDuyuruDosyasi tdydd = new TezDosyaYuklemeDuyuruDosyasi
                    {
                        DosyaAdi = System.IO.Path.GetFileNameWithoutExtension(dosyaAdi),
                        DosyaYolu = dosyaAdi,
                        TezDosyaYuklemeDuyurusuID = tezDosyaYuklemeDuyurusuID
                    };
                    new TezDosyaYuklemeDuyuruDosyasiService().TezDosyaYuklemeDuyuruDosyalariEkle(tdydd);
                }
            }
        }

        public ActionResult TezDosyaYuklemeDuyurusuGuncelle(int ID)
        {
            TezDosyaYuklemeDuyurusu tdyd = new TezDosyaYuklemeDuyuruService().TezDosyaYuklemeDuyurulariGetir().Where(x => x.ID == ID).SingleOrDefault();
            if (tdyd != null)
            {
                TezDosyaYuklemeDuyurusuViewModel model = new TezDosyaYuklemeDuyurusuViewModel
                {
                    ID = tdyd.ID,
                    Aciklama = tdyd.Aciklama,
                    Adi = tdyd.Adi,
                    SonDosyaYuklemeTarihi = tdyd.SonDosyaYuklemeTarihi,
                    TezDonemi = new TezDonemi(),
                    TezDersKod = new TezDersKod()
                };

                model.TezDonemi.ID = tdyd.TezDonemID;
                model.TezDersKod.ID = tdyd.DersKoduID;

                TempData["TezDonemi"] = new TezDonemService().TezDonemiGetir().Last();
                ViewBag.TezDonemi = TempData["TezDonemi"];

                TempData["TezDersKodu"] = new TezDersKodService().TezDersKodlariGetir();
                ViewBag.TezDersKodu = TempData["TezDersKodu"];

                TempData["TezDosyaYuklemeDuyuruDosyalari"] = new TezDosyaYuklemeDuyuruDosyasiService().TezDosyaYuklemeDuyuruDosyalariGetir().Where(x => x.TezDosyaYuklemeDuyurusuID == ID).ToList();
                ViewBag.TezDosyaYuklemeDuyuruDosyalari = TempData["TezDosyaYuklemeDuyuruDosyalari"];

                return View(model);
            }
            return RedirectToAction("TezDosyaYuklemeDuyurulari");
        }

        [HttpPost]
        public ActionResult TezDosyaYuklemeDuyurusuGuncelle(TezDosyaYuklemeDuyurusuViewModel model, List<HttpPostedFileBase> files)
        {
            if (!ModelState.IsValid)
            {

                TempData["TezDonemi"] = new TezDonemService().TezDonemiGetir().Last();
                ViewBag.TezDonemi = TempData["TezDonemi"];

                TempData["TezDersKodu"] = new TezDersKodService().TezDersKodlariGetir();
                ViewBag.TezDersKodu = TempData["TezDersKodu"];

                return View(model);
            }
            TezDosyaYuklemeDuyuruService client = new TezDosyaYuklemeDuyuruService();

            TezDosyaYuklemeDuyurusu duyuru = client.TezDosyaYuklemeDuyurulariGetir().Where(x => x.ID == model.ID).SingleOrDefault();
            if (duyuru != null)
            {
                duyuru.ID = model.ID;
                duyuru.Adi = model.Adi;
                duyuru.Aciklama = model.Aciklama;
                duyuru.Aktif = true;
                duyuru.DersKoduID = model.TezDersKod.ID;
                duyuru.SonDosyaYuklemeTarihi = model.SonDosyaYuklemeTarihi;
                duyuru.TezDonemID = model.TezDonemi.ID;

                client.TezDosyaYuklemeDuyurusuGuncelle(duyuru);
                TezDosyaYuklemeDuyurusuDosyasiEkle(files, model.ID);

                ViewBag.Mesaj = $"<div class='alert alert-success'><strong>Başarılı!</strong> Tez Dosya Duyurusu Başarıyla Güncellendi... </div>";

                TempData["TezDonemi"] = new TezDonemService().TezDonemiGetir().Last();
                ViewBag.TezDonemi = TempData["TezDonemi"];

                TempData["TezDersKodu"] = new TezDersKodService().TezDersKodlariGetir();
                ViewBag.TezDersKodu = TempData["TezDersKodu"];


                TempData["TezDosyaYuklemeDuyuruDosyalari"] = new TezDosyaYuklemeDuyuruDosyasiService().TezDosyaYuklemeDuyuruDosyalariGetir().Where(x => x.TezDosyaYuklemeDuyurusuID == model.ID).ToList();
                ViewBag.TezDosyaYuklemeDuyuruDosyalari = TempData["TezDosyaYuklemeDuyuruDosyalari"];

                return View(model);
            }
            return View();
        }

        public string TezDosyaYuklemeDuyurusuSil(int ID)
        {
            var mesaj = "";
            TezDosyaYuklemeDuyuruService client = new TezDosyaYuklemeDuyuruService();
            TezDosyaYuklemeDuyurusu tezDosyaYuklemeDuyurusu = client.TezDosyaYuklemeDuyurulariGetir().Where(x => x.ID == ID).SingleOrDefault();
            if (tezDosyaYuklemeDuyurusu != null)
            {
                client.TezDosyaYuklemeDuyurusuSil(tezDosyaYuklemeDuyurusu);

                mesaj = JsonConvert.SerializeObject(new { durum = "OK", mesaj = "Tez Dosya Yükleme Duyurusu Silindi" });
            }
            return mesaj;
        }

        #endregion

        [HttpPost]
        public string DosyaSil(int ID)
        {
            string mesaj = string.Empty;
            TezDosyaYuklemeDuyuruDosyasi doc = new TezDosyaYuklemeDuyuruDosyasiService().TezDosyaYuklemeDuyuruDosyalariGetir().SingleOrDefault(x => x.ID == ID);

            if (doc != null)
            {
                string dosya = $"{Server.MapPath(@"\Uploads\" + Helpers.FileUpload.UploadFolder.DuyuruYuklemeDosyasi)}\\{doc.DosyaYolu}";

                new TezDosyaYuklemeDuyuruDosyasiService().TezDosyaYuklemeDuyuruDosyalariSil(doc);
                if (System.IO.File.Exists(dosya))
                {
                    System.IO.File.Delete(dosya);
                }

                mesaj = JsonConvert.SerializeObject(new { durum = "OK", mesaj = "Dosya Silindi" });
            }

            return mesaj;
        }




        #region Private Methodlar
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

        private bool OgrenciNoKontrol(string ogrenciNo, int ID = 0)
        {
            OgrenciService client = new OgrenciService();
            if (ID == 0)
            {
                return client.OgrencileriGetir().ToList().Where(x => x.OgrenciNumarasi == ogrenciNo).FirstOrDefault() != null ? false : true;
            }
            return client.OgrencileriGetir().ToList().Where(x => x.OgrenciNumarasi == ogrenciNo && x.ID != ID).FirstOrDefault() != null ? false : true;
        }

        private bool KullaniciKontrol(string mail, int ID = 0)
        {
            KullaniciService client = new KullaniciService();
            if (ID == 0)
            {
                return client.KullanicilariGetir().ToList().Where(x => x.Mail == mail).FirstOrDefault() != null ? false : true;
            }
            return client.KullanicilariGetir().ToList().Where(x => x.Mail == mail && x.ID != ID).FirstOrDefault() != null ? false : true;
        }

        private int KullaniciEkle(KullaniciBaseModel model)
        {
            KullaniciService client = new KullaniciService();

            Kullanici kul = new Kullanici
            {
                Ad = model.Ad,
                Fotograf = model.Fotograf,
                Mail = model.Mail,
                RolID = model.RolID,
                Soyad = model.Soyad,
                Sifre = model.Sifre
            };

            return client.KullaniciEkle(kul).ID;
        }

        private void KullaniciGuncelle(KullaniciBaseModel model)
        {
            KullaniciService client = new KullaniciService();
            Kullanici _kul = client.KullanicilariGetir().Where(x => x.ID == model.ID).SingleOrDefault();
            if (_kul != null)
            {
                _kul.Ad = model.Ad;
                _kul.Mail = model.Mail;
                _kul.Soyad = model.Soyad;
                _kul.Sifre = model.Sifre;
            }

            if (model.Fotograf != null) _kul.Fotograf = model.Fotograf;

            client.KullaniciGuncelle(_kul);
        }

        private void OgretmenEkle(int kullaniciID, string unvan)
        {
            OgretmenService client = new OgretmenService();
            Ogretmen ogr = new Ogretmen
            {
                ID = kullaniciID,
                Unvan = unvan
            };
            client.OgretmenEkle(ogr);
        }

        private void OgretmenGuncelle(int kullaniciID, string unvan)
        {
            OgretmenService client = new OgretmenService();
            Ogretmen _ogr = client.OgretmenleriGetir().Where(x => x.ID == kullaniciID).SingleOrDefault();
            if (_ogr != null)
            {
                _ogr.Unvan = unvan;
            }
            client.OgretmenGuncelle(_ogr);
        }

        private void OgrenciEkle(int kullaniciID, string ogrenciNo)
        {
            OgrenciService client = new OgrenciService();
            Ogrenci ogr = new Ogrenci
            {
                ID = kullaniciID,
                OgrenciNumarasi = ogrenciNo
            };
            client.OgrenciEkle(ogr);
        }

        private void OgrenciGuncelle(int kullaniciID, string ogrenciNo)
        {
            OgrenciService client = new OgrenciService();
            Ogrenci _ogr = client.OgrencileriGetir().Where(x => x.ID == kullaniciID).SingleOrDefault();
            if (_ogr != null)
            {
                _ogr.OgrenciNumarasi = ogrenciNo;
            }
            client.OgrenciGuncelle(_ogr);
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
        #endregion

    }
}
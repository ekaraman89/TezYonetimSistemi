using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
    //[OgrenciRole]
    public class OgrenciController : Controller
    {
        private int OgrenciID;
        public OgrenciController()
        {
            if ((Kullanici)Helpers.CacheProvider.CachedenOku("kullanici") != null)
            {
                OgrenciID = ((Kullanici)Helpers.CacheProvider.CachedenOku("kullanici")).ID;
            }
        }


        public ActionResult Index()
        {
            if (OgrenciID == 0) OgrenciID = ((Kullanici)Helpers.CacheProvider.CachedenOku("kullanici")).ID;

            int[] ogrenciTezleri = new TezOgrenciService().TezOgrencileriGetir().Where(x => x.OgrenciID == OgrenciID).Select(x => x.TezID).ToArray();
            List<TezDosyaYuklemeDuyurusuViewModel> lst = new List<TezDosyaYuklemeDuyurusuViewModel>();
            if (ogrenciTezleri.Length > 0)
            {
                Tez item = new TezService().TezleriGetir().Where(x => ogrenciTezleri.Contains(x.ID)).LastOrDefault();

                lst = new TezDosyaYuklemeDuyuruService().TezDosyaYuklemeDuyurulariGetir().
                   Where(x => x.TezDonemID == item.TezDonemID && x.DersKoduID == item.TezKodID).ToList().
                   Select(x => new TezDosyaYuklemeDuyurusuViewModel()
                   {
                       Adi = x.Adi,
                       Aciklama = x.Aciklama,
                       SonDosyaYuklemeTarihi = x.SonDosyaYuklemeTarihi,
                       ID = x.ID
                   }).ToList();
            }
            return View(lst);

        }


        /// <summary>
        /// Tez duyurusuna yüklenen dosyaları gösterir
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult TezDosyaYukle(int? ID)
        {
            if (ID != null)
            {
                TezDosyaYuklemeDuyurusu duyuru = new TezDosyaYuklemeDuyuruService().TezDosyaYuklemeDuyurulariGetir().Where(x => x.ID == ID).SingleOrDefault();
                int[] ogrenciTezleri = new TezOgrenciService().TezOgrencileriGetir().Where(x => x.OgrenciID == OgrenciID).Select(x => x.TezID).ToArray();
                Tez tez = new TezService().TezleriGetir().LastOrDefault(x => x.TezKodID == duyuru.DersKoduID && x.TezDonemID == duyuru.TezDonemID && ogrenciTezleri.Contains(x.ID));
                List<TezDosyaYukleme> _tezDosyaYukleme = new TezDosyaYuklemeService().TezDosyaYuklemeGetir().Where(x => x.TezID == tez.ID).ToList();

                if (tez != null)
                {
                    TezDosyaYuklemeViewModel model = new TezDosyaYuklemeViewModel
                    {
                        Tez = tez,
                        TezDosyaYuklemeDuyurusu = duyuru,
                        TezDosyaYukleme = _tezDosyaYukleme.
                            Select(y => new TezDosyaYukleme
                            {
                                ID = y.ID,
                                DosyaAciklama = y.DosyaAciklama,
                                DosyaAdi = y.DosyaAdi,
                            }).ToList()
                    };
                    ViewBag.DuyuruDosyalari = new TezDosyaYuklemeDuyuruDosyasiService().TezDosyaYuklemeDuyuruDosyalariGetir().Where(x => x.TezDosyaYuklemeDuyurusuID == ID).ToList();
                    return View(model);
                }

                return View();
            }
            ViewBag.Mesaj = $"<div class='alert alert-danger'><strong>Hata!</strong> Dosya eklenecek tez bulunamadı... </div>";
            return View();
        }

        [HttpPost]
        public ActionResult TezDosyaYukle(TezDosyaYuklemeViewModel model, List<HttpPostedFileBase> files)
        {
            model.TezDosyaYukleme = new TezDosyaYuklemeService().TezDosyaYuklemeGetir().Where(x => x.TezID == model.Tez.ID && x.TezDosyaYuklemeDuyuruID == model.TezDosyaYuklemeDuyurusu.ID).ToList();

            if (ModelState.IsValid)
            {
                if (files.Count() > 0 && files[0] != null)
                {
                    if (new TezDosyaYuklemeDuyuruService().TezDosyaYuklemeDuyurulariGetir().SingleOrDefault(x => x.ID == model.TezDosyaYuklemeDuyurusu.ID).SonDosyaYuklemeTarihi > DateTime.Now)
                    {
                        foreach (var file in files)
                        {
                            string dosyaAdi = Helpers.FileUpload.FileName(file, Helpers.FileUpload.UploadFolder.TezDosya);
                            TezDosyaYukleme tdy = new TezDosyaYukleme
                            {
                                YuklenenOgrenciID = OgrenciID,
                                TezID = model.Tez.ID,
                                TezDosyaYuklemeDuyuruID = model.TezDosyaYuklemeDuyurusu.ID,
                                DosyaAciklama = model.DosyaAciklama,
                                DosyaAdi =Path.GetFileNameWithoutExtension(dosyaAdi),
                                DosyaYolu = dosyaAdi
                            };
                            new TezDosyaYuklemeService().TezDosyaYuklemeEkle(tdy);
                        }
                        ViewBag.Mesaj = $"<div class='alert alert-success'><strong>Başarılı!</strong> Dosya eklendi </div>";
                    }
                    else
                    {
                        ViewBag.Mesaj = $"<div class='alert alert-danger'><strong>Hata!</strong> Dosya yükleme zamanı dolduğu için dosya yükleyemezsiniz </div>";
                    }
                }
                else
                {
                    ViewBag.Mesaj = $"<div class='alert alert-danger'><strong>Hata!</strong> Dosya seçilmedi </div>";
                }
            }

            return View(model);
        }


        public ActionResult Tezlerim()
        {
            int[] ogrenciTezleri = new TezOgrenciService().TezOgrencileriGetir().Where(x => x.OgrenciID == OgrenciID).Select(x => x.TezID).ToArray();

            List<TezViewModel> lst = new List<TezViewModel>();

            foreach (Tez item in new TezService().TezleriGetir().Where(x => ogrenciTezleri.Contains(x.ID)).ToList())
            {
                TezViewModel tvm = new TezViewModel
                {
                    ID = item.ID,
                    TezAdi = item.TezAdi,
                    TezDonemID = item.TezDonemID,
                    TezKodID = item.TezKodID,
                    OgretmenID = item.OgretmenID,
                    TezDonemi = new TezDonemService().TezDonemiGetir().Where(x => x.ID == item.TezDonemID).SingleOrDefault().TezDonemAdi
                };
                OgretmenViewModel tmp = OgretmenleriGetir().SingleOrDefault(x => x.ID == item.OgretmenID);
                tvm.OgretimGorevlisi = $"{tmp.Unvan} {tmp.Ad} {tmp.Soyad}";
                lst.Add(tvm);
            }

            return View(lst);

        }

        /// <summary>
        /// giriş yapmış tüm rollere açık methot
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        //[AdminRole]
        public ActionResult Detay(int? ID)
        {
            if (ID != null)
            {
                Ogrenci ogr = new OgrenciService().OgrencileriGetir().SingleOrDefault(x => x.ID == ID);
                Kullanici kul = new KullaniciService().KullanicilariGetir().SingleOrDefault(x => x.ID == ID);
                if (kul != null && ogr != null)
                {
                    OgrenciViewModel model = new OgrenciViewModel
                    {
                        ID = ogr.ID,
                        Ad = kul.Ad,
                        Fotograf = kul.Fotograf,
                        Mail = kul.Mail,
                        OgrenciNo = ogr.OgrenciNumarasi,
                        Soyad = kul.Soyad
                    };
                    return View(model);
                }
            }
            ViewBag.Mesaj = "Kullanıcı bulunamadı...";
            return null;
        }





        public ActionResult TezDetay(int ID)
        {
            ViewBag.ID = ID;
            return View();
        }

        [HttpPost]
        public string DosyaSil(int ID)
        {
            string mesaj = string.Empty;
            TezDosyaYukleme doc = new TezDosyaYuklemeService().TezDosyaYuklemeGetir().SingleOrDefault(x => x.ID == ID);

            if (doc != null)
            {
                string dosya = $"{Server.MapPath(@"\Uploads\" + Helpers.FileUpload.UploadFolder.TezDosya)}\\{doc.DosyaYolu}";
                if (doc.YuklenenOgrenciID == OgrenciID)
                {
                    new TezDosyaYuklemeService().TezDosyaYuklemeSil(doc);
                    System.IO.File.Delete(dosya);
                    mesaj = JsonConvert.SerializeObject(new { durum = "OK", mesaj = "Dosya Silindi" });
                }
                else
                {
                    mesaj = JsonConvert.SerializeObject(new { durum = "NO", mesaj = "Sadece kendi yüklediğiniz dosyaları silebilirsiniz." });
                }
            }

            return mesaj;
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
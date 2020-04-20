using System.Web.Mvc;
using TezYonetimSistemi.Model;
using TezYonetimSistemi.Services;
using TezYonetimSistemi.UI.Helpers;
using TezYonetimSistemi.UI.Models;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using TezYonetimSistemi.UI.Filters;
using System.Web;
using System.IO.Compression;
using System;

namespace TezYonetimSistemi.UI.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GirisYap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GirisYap(GirisViewModel model)
        {
            if (ModelState.IsValid)
            {
                Kullanici login = model.GirisKontrol();
                if (login != null)
                {
                    Session["kullanici"] = login;
                    switch (login.RolID)
                    {
                        case 1:
                            return RedirectToAction("Index", "Admin");
                        case 2:
                            return RedirectToAction("Index", "OgretimGorevlisi");
                        case 3:
                            return RedirectToAction("Index", "Ogrenci");
                    }

                }

                ViewBag.Mesaj = "<div class='alert alert-danger display'><button class='close' data-close='alert'></button><span> Böyle bir kullanıcı yoktur... </span></div>";
            }

            return View(model);
        }

        public ActionResult Yetki()
        {

            return View();
        }

        public ActionResult Cikis()
        {
            Session.Remove("kullanici");
            CacheProvider.CacheSil("kullanici");
            return RedirectToAction("GirisYap", "Default");
        }

        [ChildActionOnly]
        public ActionResult OgrenciTezPartialViev(int ID)
        {
            int[] ogrenciTezleri = new TezOgrenciService().TezOgrencileriGetir().Where(x => x.OgrenciID == ID).OrderByDescending(y => y.TezID).Select(x => x.TezID).ToArray();
            //Tez item = new TezService().TezleriGetir().Where(x => ogrenciTezleri.Contains(x.ID)).LastOrDefault();

            List<OgrenciTezPartialVievModel> lst = new List<OgrenciTezPartialVievModel>();
            foreach (int tezler in ogrenciTezleri)
            {
                OgrenciTezPartialVievModel model = new OgrenciTezPartialVievModel(tezler);
                lst.Add(model);
            }

            return PartialView(lst);
        }

        [ChildActionOnly]
        public ActionResult TezDetayPartialView(int ID)
        {
            return View(new OgrenciTezPartialVievModel(ID));
        }

        /// <summary>
        /// Tez e yüklenen dosyalar
        /// </summary>
        /// <param name="ID">Tez ID</param>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult TezDosyalariPartialView(int ID)
        {
            List<Kullanici> kullanicilar = new KullaniciService().KullanicilariGetir().ToList();
            TezDosyaYuklemeDuyurusu duyuru = new TezDosyaYuklemeDuyuruService().TezDosyaYuklemeDuyurulariGetir().Where(x => x.ID == ID).SingleOrDefault();
            Tez tez = new TezService().TezleriGetir().SingleOrDefault(x => x.ID == ID);

            if (tez != null)
            {
                TezDosyaYuklemeViewModel model = new TezDosyaYuklemeViewModel
                {
                    Tez = tez,
                    TezDosyaYuklemeDuyurusu = duyuru,
                    TezDosyaYukleme = new TezDosyaYuklemeService().TezDosyaYuklemeGetir().Where(x => x.TezID == tez.ID).
                        Select(y => new TezDosyaYukleme
                        {
                            ID = y.ID,
                            DosyaAciklama = y.DosyaAciklama,
                            DosyaAdi = y.DosyaAdi,
                            DosyaYolu = y.DosyaYolu,
                            TezDosyaYuklemeDuyuruID = y.TezDosyaYuklemeDuyuruID,
                            OlusturmaTarihi = y.OlusturmaTarihi,
                            YukleyenOgrenciBilgisi = $"{kullanicilar.SingleOrDefault(x => x.ID == y.YuklenenOgrenciID).Ad} {kullanicilar.SingleOrDefault(x => x.ID == y.YuklenenOgrenciID).Soyad}",
                            TezID = y.TezID,
                            YuklenenOgrenciID = y.YuklenenOgrenciID
                        }).ToList()
                };

                return View(model);
            }

            return View();

        }

        [ChildActionOnly]
        public ActionResult SoyYuklenenTezlerPartialView(List<TezViewModel> lst)
        {
            return View(lst);
        }

        [ChildActionOnly]
        public ActionResult SonYuklenenDosyalarPartialView(int? OgretmenID)
        {
            List<TezDosyaYuklemeViewModel> lst = new List<TezDosyaYuklemeViewModel>();

            if (OgretmenID == null)
            {
                lst = new TezDosyaYuklemeViewModel().TezDosyaYuklemeListesi().OrderByDescending(x => x.EklenmeTarihi).Take(10).ToList();
            }
            else
            {
                lst = new TezDosyaYuklemeViewModel().TezDosyaYuklemeListesi().Where(x => x.Ogretmen.ID == OgretmenID).OrderByDescending(x => x.EklenmeTarihi).Take(10).ToList();
            }
            ViewBag.OgretmenID = OgretmenID;

            return View(lst);
        }

        public ActionResult DosyaIndir(int ID, Helpers.FileUpload.UploadFolder file)
        {
            string retval = string.Empty;
            var doc = (dynamic)null;
            string dosya = string.Empty;
            switch (file)
            {
                case FileUpload.UploadFolder.Profil:
                    break;
                case FileUpload.UploadFolder.TezDosya:
                    doc = new TezDosyaYuklemeService().TezDosyaYuklemeGetir().SingleOrDefault(x => x.ID == ID);
                    dosya = $"{Server.MapPath(@"\Uploads\" + Helpers.FileUpload.UploadFolder.TezDosya)}\\{doc.DosyaYolu}";
                    break;
                case FileUpload.UploadFolder.DuyuruYuklemeDosyasi:
                    doc = new TezDosyaYuklemeDuyuruDosyasiService().TezDosyaYuklemeDuyuruDosyalariGetir().SingleOrDefault(x => x.ID == ID);
                    dosya = $"{Server.MapPath(@"\Uploads\" + Helpers.FileUpload.UploadFolder.DuyuruYuklemeDosyasi)}\\{doc.DosyaYolu}";
                    break;
                default:
                    break;
            }

            if (doc != null && !string.IsNullOrWhiteSpace(dosya) && System.IO.File.Exists(dosya))
            {
                return File(dosya, Path.GetExtension(dosya), doc.DosyaYolu);
            }
            return Json("Dosya Bulunamadı", JsonRequestBehavior.AllowGet);
        }

        public ActionResult TezDetay(int ID)
        {
            ViewBag.ID = ID;
            return View();
        }


        [CustomAuth]
        public ActionResult Profil()
        {
            int kullaniciID = ((Kullanici)Helpers.CacheProvider.CachedenOku("kullanici")).ID;
            ProfilViewModel model = new ProfilViewModel(kullaniciID);
            @ViewBag.Mesaj = TempData["Mesaj"];
            return View(model);
        }

        [HttpPost]
        public ActionResult ResimDegistir(HttpPostedFileBase image)
        {
            int kullaniciID = ((Kullanici)Helpers.CacheProvider.CachedenOku("kullanici")).ID;
            ProfilViewModel model = new ProfilViewModel(kullaniciID);
            if (image != null)
            {
                model.Fotograf = FileUpload.FileName(image, FileUpload.UploadFolder.Profil);
                model.Guncelle();
                TempData["Mesaj"] = $"<div class='alert alert-success'><strong>Başarılı!</strong> Profil Fotoğrafınız Başarıyla Güncellendi... </div>";
            }
            return Redirect("Profil#tab_1_2");
            //return View("Profil", model);
        }

        [HttpPost]
        public ActionResult SifreDegistir(ProfilViewModel model)
        {
            int kullaniciID = ((Kullanici)Helpers.CacheProvider.CachedenOku("kullanici")).ID;
            ProfilViewModel mevcut = new ProfilViewModel(kullaniciID);

            if (!string.IsNullOrWhiteSpace(model.Sifre))
            {
                if ((model.Sifre == model.SifreTekrar))
                {
                    if (mevcut.Sifre == model.MevcutSifre)
                    {
                        mevcut.Sifre = model.Sifre;
                        mevcut.Guncelle();
                        TempData["Mesaj"] = $"<div class='alert alert-success'><strong>Başarılı!</strong> Şifreniz Başarıyla Güncellendi... </div>";
                    }
                    else
                    {
                        TempData["Mesaj"] = $"<div class='alert alert-danger'><strong>Hata!</strong> Mevcut şifreniz yanlış </div>";
                    }
                }
                else
                {
                    TempData["Mesaj"] = $"<div class='alert alert-danger'><strong>Hata!</strong> Şifreler uyuşmuyor </div>";
                }
            }
            else
            {
                TempData["Mesaj"] = $"<div class='alert alert-danger'><strong>Hata!</strong> Şifre alanı boş olamaz </div>";
            }

            return Redirect("Profil#tab_1_3");
            //return View("Profil", mevcut);
        }

        [HttpPost]
        public ActionResult BilgileriDegistir(ProfilViewModel model)
        {
            int kullaniciID = ((Kullanici)Helpers.CacheProvider.CachedenOku("kullanici")).ID;
            ProfilViewModel mevcut = new ProfilViewModel(kullaniciID);

            if (KullaniciKontrol(model.Mail, kullaniciID))
            {
                //mevcut.Ad = model.Ad;
                //mevcut.Soyad = model.Soyad;
                mevcut.Mail = model.Mail;
                mevcut.Guncelle();
                TempData["Mesaj"] = $"<div class='alert alert-success'><strong>Başarılı!</strong> Bilgileriniz Başarıyla Güncellendi... </div>";
            }
            else
            {
                TempData["Mesaj"] = $"<div class='alert alert-danger'><strong>Hata!</strong> Mail adresi kullanılıyor... </div>";

            }
            return Redirect("Profil#tab_1_1");
            //return View("Profil", mevcut);

        }


        private bool KullaniciKontrol(string mail, int ID)
        {
            KullaniciService client = new KullaniciService();
            return client.KullanicilariGetir().ToList().Where(x => x.Mail == mail && x.ID != ID).FirstOrDefault() != null ? false : true;
        }


        [HttpPost]
        public ActionResult TopluIndir(string TezAdi, string IDs, string URL)
        {

            if (!string.IsNullOrWhiteSpace(IDs))
            {
                var intIDs = IDs.Split(',').Select(Int32.Parse).ToList();

                string[] dosyalar = new TezDosyaYuklemeService().TezDosyaYuklemeGetir().Where(x => intIDs.Contains(x.ID)).Select(y => y.DosyaYolu).ToArray();

                var filesCol = GetFile(dosyalar).ToList();

                string filepath = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/TezDosya");
                using (var memoryStream = new MemoryStream())
                {
                    using (var ziparchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                    {
                        for (int i = 0; i < filesCol.Count; i++)
                        {
                            ziparchive.CreateEntryFromFile(filepath + '\\' + filesCol[i].Name, filesCol[i].Name);
                        }
                    }
                    return File(memoryStream.ToArray(), "application/zip", TezAdi + ".zip");
                }
            }
            else
            {
                return Redirect(URL);
            }

        }


        private List<FileInfo> GetFile(string[] files)
        {
            List<FileInfo> listFiles = new List<FileInfo>();
            string fileSavePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/TezDosya");
            DirectoryInfo dirInfo = new DirectoryInfo(fileSavePath);
            int i = 0;
            foreach (var item in dirInfo.GetFiles())
            {
                if (files.Contains(item.Name))
                {
                    FileInfo file = new FileInfo(item.Name);
                    listFiles.Add(file);
                }
            }
            return listFiles;
        }


    }
}
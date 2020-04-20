using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TezYonetimSistemi.UI.Helpers
{
    public static class FileUpload
    {
        public enum UploadFolder
        {
            Profil,
            TezDosya,
            DuyuruYuklemeDosyasi
        }

        public static string FileName(HttpPostedFileBase file, UploadFolder f)
        {
            string dosyaAdi = "no_image";
            string uzanti = ".png";
            if (file != null)
            {
                dosyaAdi = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
                uzanti = System.IO.Path.GetExtension(file.FileName);

                int count = 0;
                string path = System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads"), f.ToString(), $"{dosyaAdi}{uzanti}");

                while (System.IO.File.Exists(path))
                {
                    count++;
                    dosyaAdi = $"{System.IO.Path.GetFileNameWithoutExtension(file.FileName)}({count.ToString()})";
                    path = System.IO.Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads"), f.ToString(), $"{dosyaAdi}{uzanti}");
                }

                file.SaveAs(path);
            }
            return $"{dosyaAdi}{uzanti}";
        }
    }
}
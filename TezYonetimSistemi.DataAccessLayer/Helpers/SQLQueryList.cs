namespace TezYonetimSistemi.DataAccessLayer.Helpers
{
    /// <summary>
    /// Tüm procedureler burada olacak.
    /// </summary>
    public static class SQLQueryList
    {
        #region Kullanici
        public static string KullanicilariGetir = "Select * from Kullanici";
        public static string KullaniciEkle = "spKullaniciEkle";
        public static string KullaniciGuncelle = "spKullaniciGuncelle";
        public static string KullaniciSil = "spKullaniciSil";
        #endregion

        #region Ogrenci
        public static string OgrencileriGetir = "Select * from Ogrenci";
        public static string OgrenciEkle = "spOgrenciEkle";
        public static string OgrenciGuncelle = "spOgrenciGuncelle";
        public static string OgrenciSil = "spOgrenciSil";
        #endregion

        #region Ogretmen
        public static string OgretmenleriGetir = "Select * from Ogretmen";
        public static string OgretmenEkle = "spOgretmenEkle";
        public static string OgretmenGuncelle = "spOgretmenGuncelle";
        public static string OgretmenSil = "spOgretmenSil";
        #endregion

        #region Rol
        public static string RolleriGetir = "Select * from Rol";
        public static string RolEkle = "spRolEkle";
        public static string RolGuncelle = "spRolGuncelle";
        public static string RolSil = "spRolSil";
        #endregion

        #region Tez
        public static string TezleriGetir = "Select * from Tez";
        public static string TezEkle = "spTezEkle";
        public static string TezGuncelle = "spTezGuncelle";
        public static string TezSil = "spTezSil";
        #endregion

        #region TezDosya
        public static string TezDosyalariGetir = "Select * from TezDosya";
        public static string TezDosyaEkle = "spTezDosyaEkle";
        public static string TezDosyaGuncelle = "spTezDosyaGuncelle";
        public static string TezDosyaSil = "spTezDosyaSil";
        #endregion

        #region TezDosyaYuklemeDuyurusu
        public static string TezDosyaYuklemeDuyurulariGetir = "Select * from TezDosyaYuklemeDuyurusu";
        public static string TezDosyaYuklemeDuyurusuEkle = "spTezDosyaYuklemeDuyurusuEkle";
        public static string TezDosyaYuklemeDuyurusuGuncelle = "spTezDosyaYuklemeDuyurusuGuncelle";
        public static string TezDosyaYuklemeDuyurusuSil = "spTezDosyaYuklemeDuyurusuSil";
        #endregion

        #region TezDersKod
        public static string TezDersKodlariGetir = "Select * from TezDersKod";
        public static string TezDersKodEkle = "spTezDersKodEkle";
        public static string TezDersKodGuncelle = "spTezDersKodGuncelle";
        public static string TezDersKodSil = "spTezDersKodSil";
        #endregion

        #region TezOgrenci
        public static string TezOgrencileriGetir = "Select * from TezOgrenci";
        public static string TezOgrenciEkle = "spTezOgrenciEkle";
        public static string TezOgrenciSil = "spTezOgrenciSil";
        #endregion

        public static string TezDonemiGetir = "Select * from TezDonem";

        #region TezDosyaYukleme
        public static string TezDosyaYuklemeleriGetir = "Select * from TezDosyaYukleme";
        public static string TezDosyaYuklemeEkle = "spTezDosyaYuklemeEkle";
        public static string TezDosyaYuklemeGuncelle = "TezDosyaYuklemeGuncelle";
        public static string TezDosyaYuklemeSil = "spTezDosyaYuklemeSil";

        #endregion

        #region TezDosyaYuklemeDuyuruDosyalari
        public static string TezDosyaYuklemeDuyuruDosyalariGetir = "Select * from TezDosyaYuklemeDuyuruDosyalari";
        public static string TezDosyaYuklemeDuyuruDosyalariEkle = "TezDosyaYuklemeDuyuruDosyasiEkle";
        public static string TezDosyaYuklemeDuyuruDosyalariSil = "TezDosyaYuklemeDuyuruDosyasiSil";
        #endregion


    }
}

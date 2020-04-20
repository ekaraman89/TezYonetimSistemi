using System;

namespace TezYonetimSistemi.Model
{
    public class TezDosyaYuklemeDuyurusu : BaseModel
    {
        public string Adi { get; set; }

        public DateTime SonDosyaYuklemeTarihi { get; set; }

        public bool Aktif { get; set; }

        public string Aciklama { get; set; }

        public int TezDonemID { get; set; }

        public int DersKoduID { get; set; }
    }
}

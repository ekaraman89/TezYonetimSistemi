namespace TezYonetimSistemi.Model
{
    public class TezDosya : BaseModel
    {
        public string DosyaAdi { get; set; }

        public string DosyaYolu { get; set; }

        public int OgrenciID { get; set; }

        public int TezID { get; set; }

        public int TezDosyaKontrolID { get; set; }

    }
}

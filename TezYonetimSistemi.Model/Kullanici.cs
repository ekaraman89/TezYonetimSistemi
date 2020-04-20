namespace TezYonetimSistemi.Model
{
    public class Kullanici : BaseModel
    {
        public string Mail { get; set; }

        public string Sifre { get; set; }

        public int RolID { get; set; }

        public string Ad { get; set; }

        public string Soyad { get; set; }

        public string Fotograf { get; set; }

    }
}

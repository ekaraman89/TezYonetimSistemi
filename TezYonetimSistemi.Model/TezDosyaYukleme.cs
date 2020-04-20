using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TezYonetimSistemi.Model
{
    public class TezDosyaYukleme : BaseModel
    {
        public int TezID { get; set; }

        public int TezDosyaYuklemeDuyuruID { get; set; }

        public int YuklenenOgrenciID { get; set; }
        public string YukleyenOgrenciBilgisi { get; set; }
        public string DosyaAdi { get; set; }

        public string DosyaYolu { get; set; }

        public string DosyaAciklama { get; set; }

    }
}

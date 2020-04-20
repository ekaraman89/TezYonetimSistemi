using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TezYonetimSistemi.Model
{
    public class TezDosyaYuklemeDuyuruDosyasi : BaseModel
    {
        public int TezDosyaYuklemeDuyurusuID { get; set; }

        public string DosyaAdi { get; set; }

        public string DosyaYolu { get; set; }
    }
}

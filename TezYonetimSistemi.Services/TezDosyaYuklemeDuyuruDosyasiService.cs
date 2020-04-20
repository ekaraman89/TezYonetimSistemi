using System.Collections.Generic;
using TezYonetimSistemi.DataAccessLayer;
using TezYonetimSistemi.DataAccessLayer.Repositories;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.Services
{
    public class TezDosyaYuklemeDuyuruDosyasiService : BaseService
    {
        public TezDosyaYuklemeDuyuruDosyasi TezDosyaYuklemeDuyuruDosyalariEkle(TezDosyaYuklemeDuyuruDosyasi tezDosyaYuklemeDuyuruDosyasi)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezDosyaYuklemeDuyuruDosyasiRepo = new TezDosyaYuklemeDuyuruDosyaRepository(context);

                return TezDosyaYuklemeDuyuruDosyasiRepo.TezDosyaYuklemeDuyuruDosyalariEkle(tezDosyaYuklemeDuyuruDosyasi);
            }
        }

        public IList<TezDosyaYuklemeDuyuruDosyasi> TezDosyaYuklemeDuyuruDosyalariGetir()
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezDosyaYuklemeDuyuruDosyasiRepo = new TezDosyaYuklemeDuyuruDosyaRepository(context);
                return TezDosyaYuklemeDuyuruDosyasiRepo.TezDosyaYuklemeDuyuruDosyalariGetir();
            }
        }

        public int TezDosyaYuklemeDuyuruDosyalariSil(TezDosyaYuklemeDuyuruDosyasi tezDosyaYuklemeDuyuruDosyasi)
        {

            using (var context = new DbContext(connectionFactory))
            {
                var TezDosyaYuklemeDuyuruDosyasiRepo = new TezDosyaYuklemeDuyuruDosyaRepository(context);
                return TezDosyaYuklemeDuyuruDosyasiRepo.TezDosyaYuklemeDuyuruDosyalariSil(tezDosyaYuklemeDuyuruDosyasi);
            }
        }
    }
}

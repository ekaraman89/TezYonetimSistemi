using System.Collections.Generic;
using TezYonetimSistemi.DataAccessLayer;
using TezYonetimSistemi.DataAccessLayer.Repositories;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.Services
{
    public class TezDosyaService : BaseService
    {
        public TezDosya TezDosyaEkle(TezDosya tezDosya)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezDosyaRepo = new TezDosyaRepository(context);
                return TezDosyaRepo.TezDosyaEkle(tezDosya);
            }
        }

        public TezDosya TezDosyaGuncelle(TezDosya tezDosya)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezDosyaRepo = new TezDosyaRepository(context);
                return TezDosyaRepo.TezDosyaGuncelle(tezDosya);
            }
        }

        public IList<TezDosya> TezDosyaleriGetir()
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezDosyaRepo = new TezDosyaRepository(context);
                return TezDosyaRepo.TezDosyalariGetir();
            }
        }

        public int TezDosyaSil(TezDosya tezDosya)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezDosyaRepo = new TezDosyaRepository(context);
                return TezDosyaRepo.TezDosyaSil(tezDosya);
            }
        }
    }
}

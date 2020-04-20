using System.Collections.Generic;
using TezYonetimSistemi.DataAccessLayer;
using TezYonetimSistemi.DataAccessLayer.Repositories;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.Services
{
    public class TezDersKodService : BaseService
    {
        public TezDersKod TezDersKodEkle(TezDersKod tezKod)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezKodRepo = new TezDersKodRepository(context);
                return TezKodRepo.TezDersKodEkle(tezKod);
            }
        }

        public TezDersKod TezDersKodGuncelle(TezDersKod tezKod)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezKodRepo = new TezDersKodRepository(context);
                return TezKodRepo.TezDersKodGuncelle(tezKod);
            }
        }

        public IList<TezDersKod> TezDersKodlariGetir()
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezKodRepo = new TezDersKodRepository(context);
                return TezKodRepo.TezDersKodlariGetir();
            }
        }

        public int TezDersKodSil(TezDersKod tezKod)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezKodRepo = new TezDersKodRepository(context);
                return TezKodRepo.TezDersKodSil(tezKod);
            }
        }
    }
}

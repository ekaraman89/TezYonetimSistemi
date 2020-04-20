using System.Collections.Generic;
using TezYonetimSistemi.DataAccessLayer;
using TezYonetimSistemi.DataAccessLayer.Repositories;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.Services
{
    public class TezService : BaseService
    {
        public Tez TezEkle(Tez tez)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezRepo = new TezRepository(context);
                return TezRepo.TezEkle(tez);
            }
        }

        public Tez TezGuncelle(Tez tez)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezRepo = new TezRepository(context);
                return TezRepo.TezGuncelle(tez);
            }
        }

        public IList<Tez> TezleriGetir()
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezRepo = new TezRepository(context);
                return TezRepo.TezleriGetir();
            }
        }

        public int TezSil(Tez tez)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezRepo = new TezRepository(context);
                return TezRepo.TezSil(tez);
            }
        }
    }
}

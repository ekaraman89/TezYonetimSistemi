using System.Collections.Generic;
using TezYonetimSistemi.DataAccessLayer;
using TezYonetimSistemi.DataAccessLayer.Repositories;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.Services
{
    public class TezOgrenciService : BaseService
    {
        public TezOgrenci TezOgrenciEkle(TezOgrenci tezOgrenci)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezOgrenciRepo = new TezOgrenciRepository(context);

                return TezOgrenciRepo.TezOgrenciEkle(tezOgrenci);
            }
        }

        public IList<TezOgrenci> TezOgrencileriGetir()
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezOgrenciRepo = new TezOgrenciRepository(context);

                return TezOgrenciRepo.TezOgrencileriGetir();
            }
        }

        public int TezOgrenciSil(TezOgrenci tezOgrenci)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezOgrenciRepo = new TezOgrenciRepository(context);

                return TezOgrenciRepo.TezOgrenciSil(tezOgrenci);
            }
        }
    }
}

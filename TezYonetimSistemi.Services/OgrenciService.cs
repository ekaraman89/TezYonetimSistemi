using System.Collections.Generic;
using TezYonetimSistemi.DataAccessLayer;
using TezYonetimSistemi.DataAccessLayer.Repositories;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.Services
{
    public class OgrenciService : BaseService
    {
        public Ogrenci OgrenciEkle(Ogrenci ogrenci)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var OgrenciRepo = new OgrenciRepository(context);

                return OgrenciRepo.OgrenciEkle(ogrenci);
            }


        }

        public Ogrenci OgrenciGuncelle(Ogrenci ogrenci)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var OgrenciRepo = new OgrenciRepository(context);

                return OgrenciRepo.OgrenciGuncelle(ogrenci);
            }


        }

        public IList<Ogrenci> OgrencileriGetir()
        {
            using (var context = new DbContext(connectionFactory))
            {
                var OgrenciRepo = new OgrenciRepository(context);

                return OgrenciRepo.OgrencileriGetir();
            }

        }

        public int OgrenciSil(Ogrenci ogrenci)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var OgrenciRepo = new OgrenciRepository(context);

                return OgrenciRepo.OgrenciSil(ogrenci);
            }


        }
    }
}

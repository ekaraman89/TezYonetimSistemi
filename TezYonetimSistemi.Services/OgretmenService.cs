using System.Collections.Generic;
using TezYonetimSistemi.DataAccessLayer;
using TezYonetimSistemi.DataAccessLayer.Repositories;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.Services
{
    public class OgretmenService : BaseService
    {
        public Ogretmen OgretmenEkle(Ogretmen ogretmen)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var OgretmenRepo = new OgretmenRepository(context);

                return OgretmenRepo.OgretmenEkle(ogretmen);
            }


        }

        public Ogretmen OgretmenGuncelle(Ogretmen ogretmen)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var OgretmenRepo = new OgretmenRepository(context);

                return OgretmenRepo.OgretmenGuncelle(ogretmen);
            }


        }

        public IList<Ogretmen> OgretmenleriGetir()
        {
            using (var context = new DbContext(connectionFactory))
            {
                var OgretmenRepo = new OgretmenRepository(context);

                return OgretmenRepo.OgretmenleriGetir();
            }


        }

        public int OgretmenSil(Ogretmen ogretmen)
        {
            using (var context = new DbContext(connectionFactory))
            {

                var OgretmenRepo = new OgretmenRepository(context);

                return OgretmenRepo.OgretmenSil(ogretmen);
            }

        }
    }
}

using System.Collections.Generic;
using TezYonetimSistemi.DataAccessLayer;
using TezYonetimSistemi.DataAccessLayer.Repositories;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.Services
{
    public class KullaniciService : BaseService
    {
        public Kullanici KullaniciEkle(Kullanici kullanici)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var kullaniciRepo = new KullaniciRepository(context);

                return kullaniciRepo.KullaniciEkle(kullanici);
            }
        }

        public Kullanici KullaniciGuncelle(Kullanici kullanici)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var kullaniciRepo = new KullaniciRepository(context);

                return kullaniciRepo.KullaniciGuncelle(kullanici);
            }
        }

        public IList<Kullanici> KullanicilariGetir()
        {
            using (var context = new DbContext(connectionFactory))
            {
                var kullaniciRepo = new KullaniciRepository(context);
                return kullaniciRepo.KullanicilariGetir();
            }
        }

        public int KullaniciSil(Kullanici kullanici)
        {

            using (var context = new DbContext(connectionFactory))
            {
                var kullaniciRepo = new KullaniciRepository(context);
                return kullaniciRepo.KullaniciSil(kullanici);
            }
        }
    }
}

using System.Collections.Generic;
using TezYonetimSistemi.DataAccessLayer;
using TezYonetimSistemi.DataAccessLayer.Repositories;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.Services
{
    public class TezDosyaYuklemeService : BaseService
    {
        public TezDosyaYukleme TezDosyaYuklemeEkle(TezDosyaYukleme tezDosyaYukleme)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var tezDosyaYuklemeRepo = new TezDosyaYuklemeRepository(context);

                return tezDosyaYuklemeRepo.TezDosyaYuklemeEkle(tezDosyaYukleme);
            }
        }

        public TezDosyaYukleme TezDosyaYuklemeGuncelle(TezDosyaYukleme tezDosyaYukleme)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var tezDosyaYuklemeRepo = new TezDosyaYuklemeRepository(context);

                return tezDosyaYuklemeRepo.TezDosyaYuklemeGuncelle(tezDosyaYukleme);
            }
        }

        public IList<TezDosyaYukleme> TezDosyaYuklemeGetir()
        {
            using (var context = new DbContext(connectionFactory))
            {
                var tezDosyaYuklemeRepo = new TezDosyaYuklemeRepository(context);
                return tezDosyaYuklemeRepo.TezDosyaYuklemeGetir();
            }
        }

        public int TezDosyaYuklemeSil(TezDosyaYukleme tezDosyaYukleme)
        {

            using (var context = new DbContext(connectionFactory))
            {
                var tezDosyaYuklemeRepo = new TezDosyaYuklemeRepository(context);
                return tezDosyaYuklemeRepo.TezDosyaYuklemeSil(tezDosyaYukleme);
            }
        }
    }
}

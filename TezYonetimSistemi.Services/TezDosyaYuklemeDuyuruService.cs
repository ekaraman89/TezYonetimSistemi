using System.Collections.Generic;
using TezYonetimSistemi.DataAccessLayer;
using TezYonetimSistemi.DataAccessLayer.Repositories;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.Services
{
    public class TezDosyaYuklemeDuyuruService : BaseService
    {
        public TezDosyaYuklemeDuyurusu TezDosyaYuklemeDuyurusuEkle(TezDosyaYuklemeDuyurusu tezDosyaKontrol)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezDosyaKontrolRepo = new TezDosyaYuklemeDuyurusuRepository(context);
                return TezDosyaKontrolRepo.TezDosyaYuklemeDuyurusuEkle(tezDosyaKontrol);
            }
        }

        public TezDosyaYuklemeDuyurusu TezDosyaYuklemeDuyurusuGuncelle(TezDosyaYuklemeDuyurusu tezDosyaKontrol)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezDosyaKontrolRepo = new TezDosyaYuklemeDuyurusuRepository(context);
                return TezDosyaKontrolRepo.TezDosyaYuklemeDuyurusuGuncelle(tezDosyaKontrol);
            }
        }

        public IList<TezDosyaYuklemeDuyurusu> TezDosyaYuklemeDuyurulariGetir()
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezDosyaKontrolRepo = new TezDosyaYuklemeDuyurusuRepository(context);
                return TezDosyaKontrolRepo.TezDosyaYuklemeDuyurulariGetir();
            }
        }

        public int TezDosyaYuklemeDuyurusuSil(TezDosyaYuklemeDuyurusu tezDosyaKontrol)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezDosyaKontrolRepo = new TezDosyaYuklemeDuyurusuRepository(context);
                return TezDosyaKontrolRepo.TezDosyaYuklemeDuyurusuSil(tezDosyaKontrol);
            }
        }
    }
}

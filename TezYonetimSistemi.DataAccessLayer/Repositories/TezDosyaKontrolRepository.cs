using System.Collections.Generic;
using System.Data;
using System.Linq;
using TezYonetimSistemi.DataAccessLayer.Extensions;
using TezYonetimSistemi.DataAccessLayer.Helpers;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.DataAccessLayer.Repositories
{
    public class TezDosyaYuklemeDuyurusuRepository : Repository<TezDosyaYuklemeDuyurusu>
    {
        private DbContext _context;

        public TezDosyaYuklemeDuyurusuRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public IList<TezDosyaYuklemeDuyurusu> TezDosyaYuklemeDuyurulariGetir()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = SQLQueryList.TezDosyaYuklemeDuyurulariGetir;

                return ToList(command).ToList();
            }
        }

        public TezDosyaYuklemeDuyurusu TezDosyaYuklemeDuyurusuEkle(TezDosyaYuklemeDuyurusu tezDosyaKontrol)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezDosyaYuklemeDuyurusuEkle;

                command.Parameters.Add(command.CreateParameter("@Adi ", tezDosyaKontrol.Adi));
                command.Parameters.Add(command.CreateParameter("@SonDosyaYuklemeTarihi ", tezDosyaKontrol.SonDosyaYuklemeTarihi));
                command.Parameters.Add(command.CreateParameter("@Aktif ", tezDosyaKontrol.Aktif));
                command.Parameters.Add(command.CreateParameter("@Aciklama ", tezDosyaKontrol.Aciklama));
                command.Parameters.Add(command.CreateParameter("@TezDonemID ", tezDosyaKontrol.TezDonemID));
                command.Parameters.Add(command.CreateParameter("@DersKoduID ", tezDosyaKontrol.DersKoduID));

                return ToList(command).FirstOrDefault();
            }
        }

        public TezDosyaYuklemeDuyurusu TezDosyaYuklemeDuyurusuGuncelle(TezDosyaYuklemeDuyurusu tezDosyaKontrol)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezDosyaYuklemeDuyurusuGuncelle;

                command.Parameters.Add(command.CreateParameter("@ID ", tezDosyaKontrol.ID));
                command.Parameters.Add(command.CreateParameter("@Adi ", tezDosyaKontrol.Adi));
                command.Parameters.Add(command.CreateParameter("@SonDosyaYuklemeTarihi ", tezDosyaKontrol.SonDosyaYuklemeTarihi));
                command.Parameters.Add(command.CreateParameter("@Aktif ", tezDosyaKontrol.Aktif));
                command.Parameters.Add(command.CreateParameter("@Aciklama ", tezDosyaKontrol.Aciklama));
                command.Parameters.Add(command.CreateParameter("@TezDonemID ", tezDosyaKontrol.TezDonemID));
                command.Parameters.Add(command.CreateParameter("@DersKoduID ", tezDosyaKontrol.DersKoduID));

                return ToList(command).FirstOrDefault();
            }
        }

        public int TezDosyaYuklemeDuyurusuSil(TezDosyaYuklemeDuyurusu tezDosyaKontrol)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezDosyaYuklemeDuyurusuSil;

                command.Parameters.Add(command.CreateParameter("@ID", tezDosyaKontrol.ID));

                return Execute(command);
            }
        }
    }
}

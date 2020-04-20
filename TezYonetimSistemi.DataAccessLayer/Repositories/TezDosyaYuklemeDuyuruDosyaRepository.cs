using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TezYonetimSistemi.DataAccessLayer.Extensions;
using TezYonetimSistemi.DataAccessLayer.Helpers;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.DataAccessLayer.Repositories
{
    public class TezDosyaYuklemeDuyuruDosyaRepository : Repository<TezDosyaYuklemeDuyuruDosyasi>
    {
        private DbContext _context;
        public TezDosyaYuklemeDuyuruDosyaRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public IList<TezDosyaYuklemeDuyuruDosyasi> TezDosyaYuklemeDuyuruDosyalariGetir()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = SQLQueryList.TezDosyaYuklemeDuyuruDosyalariGetir;

                return ToList(command).ToList();
            }
        }

        public TezDosyaYuklemeDuyuruDosyasi TezDosyaYuklemeDuyuruDosyalariEkle(TezDosyaYuklemeDuyuruDosyasi tezDosyaYuklemeDuyuruDosyasi)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezDosyaYuklemeDuyuruDosyalariEkle;

                command.Parameters.Add(command.CreateParameter("@DosyaAdi", tezDosyaYuklemeDuyuruDosyasi.DosyaAdi));
                command.Parameters.Add(command.CreateParameter("@DosyaYolu", tezDosyaYuklemeDuyuruDosyasi.DosyaYolu));
                command.Parameters.Add(command.CreateParameter("@TezDosyaYuklemeDuyurusuID", tezDosyaYuklemeDuyuruDosyasi.TezDosyaYuklemeDuyurusuID));
                return ToList(command).FirstOrDefault();
            }
        }

        public int TezDosyaYuklemeDuyuruDosyalariSil(TezDosyaYuklemeDuyuruDosyasi tezDosyaYuklemeDuyuruDosyasi)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezDosyaYuklemeDuyuruDosyalariSil;

                command.Parameters.Add(command.CreateParameter("@ID", tezDosyaYuklemeDuyuruDosyasi.ID));

                return Execute(command);
            }
        }

    }
}

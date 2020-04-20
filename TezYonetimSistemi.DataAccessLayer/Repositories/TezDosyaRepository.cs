using System.Collections.Generic;
using System.Data;
using System.Linq;
using TezYonetimSistemi.DataAccessLayer.Extensions;
using TezYonetimSistemi.DataAccessLayer.Helpers;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.DataAccessLayer.Repositories
{
    public class TezDosyaRepository : Repository<TezDosya>
    {
        private DbContext _context;
        public TezDosyaRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public IList<TezDosya> TezDosyalariGetir()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = SQLQueryList.TezDosyalariGetir;

                return ToList(command).ToList();
            }
        }

        public TezDosya TezDosyaEkle(TezDosya tezDosya)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezDosyaEkle;

                command.Parameters.Add(command.CreateParameter("@DosyaAdi", tezDosya.ID));
                command.Parameters.Add(command.CreateParameter("@DosyaYolu", tezDosya.ID));
                command.Parameters.Add(command.CreateParameter("@OgrenciID", tezDosya.ID));
                command.Parameters.Add(command.CreateParameter("@TezID", tezDosya.ID));
                command.Parameters.Add(command.CreateParameter("@TezDosyaKontrolID", tezDosya.ID));

                return ToList(command).FirstOrDefault();
            }
        }

        public TezDosya TezDosyaGuncelle(TezDosya tezDosya)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezDosyaGuncelle;

                command.Parameters.Add(command.CreateParameter("@ID ", tezDosya.ID));
                command.Parameters.Add(command.CreateParameter("@DosyaAdi", tezDosya.ID));
                command.Parameters.Add(command.CreateParameter("@DosyaYolu", tezDosya.ID));
                command.Parameters.Add(command.CreateParameter("@OgrenciID", tezDosya.ID));
                command.Parameters.Add(command.CreateParameter("@TezID", tezDosya.ID));
                command.Parameters.Add(command.CreateParameter("@TezDosyaKontrolID", tezDosya.ID));
                return ToList(command).FirstOrDefault();
            }
        }

        public int TezDosyaSil(TezDosya tezDosya)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezDosyaSil;

                command.Parameters.Add(command.CreateParameter("@ID", tezDosya.ID));

                return Execute(command);
            }
        }
    }
}

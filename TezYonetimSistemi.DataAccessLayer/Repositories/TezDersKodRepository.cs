using System.Collections.Generic;
using System.Data;
using System.Linq;
using TezYonetimSistemi.DataAccessLayer.Extensions;
using TezYonetimSistemi.DataAccessLayer.Helpers;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.DataAccessLayer.Repositories
{
    public class TezDersKodRepository : Repository<TezDersKod>
    {
        private DbContext _context;
        public TezDersKodRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public IList<TezDersKod> TezDersKodlariGetir()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = SQLQueryList.TezDersKodlariGetir;

                return ToList(command).ToList();
            }
        }

        public TezDersKod TezDersKodEkle(TezDersKod tezkod)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezDersKodEkle;

                command.Parameters.Add(command.CreateParameter("@Adi ", tezkod.ID));
                command.Parameters.Add(command.CreateParameter("@Kod", tezkod.Kod));
                command.Parameters.Add(command.CreateParameter("@Aciklama", tezkod.Aciklama));

                return ToList(command).FirstOrDefault();
            }
        }

        public TezDersKod TezDersKodGuncelle(TezDersKod tezkod)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezDersKodGuncelle;

                command.Parameters.Add(command.CreateParameter("@ID ", tezkod.ID));
                command.Parameters.Add(command.CreateParameter("@Adi ", tezkod.ID));
                command.Parameters.Add(command.CreateParameter("@Kod", tezkod.Kod));
                command.Parameters.Add(command.CreateParameter("@Aciklama", tezkod.Aciklama));

                return ToList(command).FirstOrDefault();
            }
        }

        public int TezDersKodSil(TezDersKod tezkod)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezDersKodSil;

                command.Parameters.Add(command.CreateParameter("@ID", tezkod.ID));

                return Execute(command);
            }
        }

    }
}

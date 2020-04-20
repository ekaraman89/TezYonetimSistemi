using System.Collections.Generic;
using System.Data;
using System.Linq;
using TezYonetimSistemi.DataAccessLayer.Extensions;
using TezYonetimSistemi.DataAccessLayer.Helpers;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.DataAccessLayer.Repositories
{
    public class TezRepository : Repository<Tez>
    {
        private DbContext _context;
        public TezRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public IList<Tez> TezleriGetir()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = SQLQueryList.TezleriGetir;

                return ToList(command).ToList();
            }
        }

        public Tez TezEkle(Tez tez)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezEkle;

                command.Parameters.Add(command.CreateParameter("@TezAdi", tez.TezAdi));
                command.Parameters.Add(command.CreateParameter("@OgretmenID", tez.OgretmenID));
                command.Parameters.Add(command.CreateParameter("@TezKodID", tez.TezKodID));
                command.Parameters.Add(command.CreateParameter("@TezDonemID", tez.TezDonemID));

                return ToList(command).FirstOrDefault();
            }
        }

        public Tez TezGuncelle(Tez tez)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezGuncelle;

                command.Parameters.Add(command.CreateParameter("@ID ", tez.ID));
                command.Parameters.Add(command.CreateParameter("@TezAdi", tez.TezAdi));
                command.Parameters.Add(command.CreateParameter("@OgretmenID", tez.OgretmenID));
                command.Parameters.Add(command.CreateParameter("@TezKodID", tez.TezKodID));
                command.Parameters.Add(command.CreateParameter("@TezDonemID", tez.TezDonemID));


                return ToList(command).FirstOrDefault();
            }
        }

        public int TezSil(Tez tez)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezSil;

                command.Parameters.Add(command.CreateParameter("@ID", tez.ID));

                return Execute(command);
            }
        }
    }
}

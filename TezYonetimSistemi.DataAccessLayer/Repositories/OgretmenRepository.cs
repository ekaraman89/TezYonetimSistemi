using System.Collections.Generic;
using System.Data;
using System.Linq;
using TezYonetimSistemi.DataAccessLayer.Extensions;
using TezYonetimSistemi.DataAccessLayer.Helpers;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.DataAccessLayer.Repositories
{
    public class OgretmenRepository : Repository<Ogretmen>
    {
        private DbContext _context;
        public OgretmenRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public IList<Ogretmen> OgretmenleriGetir()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = SQLQueryList.OgretmenleriGetir;

                return ToList(command).ToList();
            }
        }

        public Ogretmen OgretmenEkle(Ogretmen ogretmen)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.OgretmenEkle;

                command.Parameters.Add(command.CreateParameter("@ID ", ogretmen.ID));
                command.Parameters.Add(command.CreateParameter("@Unvan", ogretmen.Unvan));

                return ToList(command).FirstOrDefault();
            }
        }

        public Ogretmen OgretmenGuncelle(Ogretmen ogretmen)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.OgretmenGuncelle;

                command.Parameters.Add(command.CreateParameter("@ID ", ogretmen.ID));
                command.Parameters.Add(command.CreateParameter("@Unvan", ogretmen.Unvan));

                return ToList(command).FirstOrDefault();
            }
        }

        public int OgretmenSil(Ogretmen ogretmen)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.OgretmenSil;

                command.Parameters.Add(command.CreateParameter("@ID", ogretmen.ID));

                return Execute(command);
            }
        }
    }
}

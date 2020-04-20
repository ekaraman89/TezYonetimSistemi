using System.Collections.Generic;
using System.Data;
using System.Linq;
using TezYonetimSistemi.DataAccessLayer.Extensions;
using TezYonetimSistemi.DataAccessLayer.Helpers;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.DataAccessLayer.Repositories
{
    public class OgrenciRepository : Repository<Ogrenci>
    {
        private DbContext _context;
        public OgrenciRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public IList<Ogrenci> OgrencileriGetir()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = SQLQueryList.OgrencileriGetir;

                return ToList(command).ToList();
            }
        }

        public Ogrenci OgrenciEkle(Ogrenci ogrenci)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.OgrenciEkle;

                command.Parameters.Add(command.CreateParameter("@ID ", ogrenci.ID));
                command.Parameters.Add(command.CreateParameter("@OgrenciNumarasi", ogrenci.OgrenciNumarasi));
               
                return ToList(command).FirstOrDefault();
            }
        }

        public Ogrenci OgrenciGuncelle(Ogrenci ogrenci)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.OgrenciGuncelle;

                command.Parameters.Add(command.CreateParameter("@ID ", ogrenci.ID));
                command.Parameters.Add(command.CreateParameter("@OgrenciNumarasi", ogrenci.OgrenciNumarasi));

                return ToList(command).FirstOrDefault();
            }
        }

        public int OgrenciSil(Ogrenci ogrenci)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.OgrenciSil;

                command.Parameters.Add(command.CreateParameter("@ID", ogrenci.ID));

                return Execute(command);
            }
        }
    }
}

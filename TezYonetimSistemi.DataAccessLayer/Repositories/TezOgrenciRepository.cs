using System.Collections.Generic;
using System.Data;
using System.Linq;
using TezYonetimSistemi.DataAccessLayer.Extensions;
using TezYonetimSistemi.DataAccessLayer.Helpers;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.DataAccessLayer.Repositories
{
    public class TezOgrenciRepository : Repository<TezOgrenci>
    {
        private DbContext _context;
        public TezOgrenciRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

         public IList<TezOgrenci> TezOgrencileriGetir()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = SQLQueryList.TezOgrencileriGetir;

                return ToList(command).ToList();
            }
        }

        public TezOgrenci TezOgrenciEkle(TezOgrenci tezOgrenci)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezOgrenciEkle;

                command.Parameters.Add(command.CreateParameter("@OgrenciID ", tezOgrenci.OgrenciID));
                command.Parameters.Add(command.CreateParameter("@TezID", tezOgrenci.TezID));
               
                return ToList(command).FirstOrDefault();
            }
        }

        public int TezOgrenciSil(TezOgrenci tezOgrenci)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezOgrenciSil;

                command.Parameters.Add(command.CreateParameter("@OgrenciID ", tezOgrenci.OgrenciID));
                command.Parameters.Add(command.CreateParameter("@TezID", tezOgrenci.TezID));

                return Execute(command);
            }
        }
    }
}

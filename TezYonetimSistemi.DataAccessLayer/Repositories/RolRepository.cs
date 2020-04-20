using System.Collections.Generic;
using System.Data;
using System.Linq;
using TezYonetimSistemi.DataAccessLayer.Extensions;
using TezYonetimSistemi.DataAccessLayer.Helpers;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.DataAccessLayer.Repositories
{
    public class RolRepository:Repository<Rol>
    {
        private DbContext _context;
        public RolRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public IList<Rol> RolleriGetir()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = SQLQueryList.RolleriGetir;

                return ToList(command).ToList();
            }
        }

        public Rol RolEkle(Rol rol)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.RolEkle;

                command.Parameters.Add(command.CreateParameter("@RolAdi ", rol.RolAdi));
                command.Parameters.Add(command.CreateParameter("@Aciklama", rol.Aciklama));

                return ToList(command).FirstOrDefault();
            }
        }

        public Rol RolGuncelle(Rol rol)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.RolGuncelle;

                command.Parameters.Add(command.CreateParameter("@ID ", rol.ID));
                command.Parameters.Add(command.CreateParameter("@RolAdi ", rol.RolAdi));
                command.Parameters.Add(command.CreateParameter("@Aciklama", rol.Aciklama));

                return ToList(command).FirstOrDefault();
            }
        }

        public int RolSil(Rol rol)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.RolSil;

                command.Parameters.Add(command.CreateParameter("@ID", rol.ID));

                return Execute(command);
            }
        }
    }
}

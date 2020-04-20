using System.Collections.Generic;
using System.Data;
using System.Linq;
using TezYonetimSistemi.DataAccessLayer.Extensions;
using TezYonetimSistemi.DataAccessLayer.Helpers;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.DataAccessLayer.Repositories
{
    public class KullaniciRepository : Repository<Kullanici>
    {
        private DbContext _context;
        public KullaniciRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public IList<Kullanici> KullanicilariGetir()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = SQLQueryList.KullanicilariGetir;

                return ToList(command).ToList();
            }
        }

        public Kullanici KullaniciEkle(Kullanici kullanici)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.KullaniciEkle;

                command.Parameters.Add(command.CreateParameter("@Mail", kullanici.Mail));
                command.Parameters.Add(command.CreateParameter("@Sifre", kullanici.Sifre));
                command.Parameters.Add(command.CreateParameter("@RolID", kullanici.RolID));
                command.Parameters.Add(command.CreateParameter("@Ad", kullanici.Ad));
                command.Parameters.Add(command.CreateParameter("@Soyad", kullanici.Soyad));
                command.Parameters.Add(command.CreateParameter("@Fotograf", kullanici.Fotograf));

                return ToList(command).FirstOrDefault();
            }
        }

        public Kullanici KullaniciGuncelle(Kullanici kullanici)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.KullaniciGuncelle;

                command.Parameters.Add(command.CreateParameter("@ID", kullanici.ID));
                command.Parameters.Add(command.CreateParameter("@Mail", kullanici.Mail));
                command.Parameters.Add(command.CreateParameter("@Sifre", kullanici.Sifre));
                command.Parameters.Add(command.CreateParameter("@RolID", kullanici.RolID));
                command.Parameters.Add(command.CreateParameter("@Ad", kullanici.Ad));
                command.Parameters.Add(command.CreateParameter("@Soyad", kullanici.Soyad));
                command.Parameters.Add(command.CreateParameter("@Fotograf", kullanici.Fotograf));

                return ToList(command).FirstOrDefault();
            }
        }

        public int KullaniciSil(Kullanici kullanici)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.KullaniciSil;

                command.Parameters.Add(command.CreateParameter("@ID", kullanici.ID));

                return Execute(command);
            }
        }
    }
}

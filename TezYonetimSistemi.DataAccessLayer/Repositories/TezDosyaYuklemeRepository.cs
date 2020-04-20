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
    public class TezDosyaYuklemeRepository : Repository<TezDosyaYukleme>
    {
        private DbContext _context;
        public TezDosyaYuklemeRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public IList<TezDosyaYukleme> TezDosyaYuklemeGetir()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = SQLQueryList.TezDosyaYuklemeleriGetir;

                return ToList(command).ToList();
            }
        }

        public TezDosyaYukleme TezDosyaYuklemeEkle(TezDosyaYukleme tezDosyaYukleme)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezDosyaYuklemeEkle;

                command.Parameters.Add(command.CreateParameter("@TezID", tezDosyaYukleme.TezID));
                command.Parameters.Add(command.CreateParameter("@TezDosyaYuklemeDuyuruID", tezDosyaYukleme.TezDosyaYuklemeDuyuruID));
                command.Parameters.Add(command.CreateParameter("@YuklenenOgrenciID", tezDosyaYukleme.YuklenenOgrenciID));
                command.Parameters.Add(command.CreateParameter("@DosyaAdi", tezDosyaYukleme.DosyaAdi));
                command.Parameters.Add(command.CreateParameter("@DosyaYolu", tezDosyaYukleme.DosyaYolu));
                command.Parameters.Add(command.CreateParameter("@DosyaAciklama", tezDosyaYukleme.DosyaAciklama));

                return ToList(command).FirstOrDefault();
            }
        }

        public TezDosyaYukleme TezDosyaYuklemeGuncelle(TezDosyaYukleme tezDosyaYukleme)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezDosyaYuklemeGuncelle;

                command.Parameters.Add(command.CreateParameter("@ID ", tezDosyaYukleme.ID));
                command.Parameters.Add(command.CreateParameter("@TezID", tezDosyaYukleme.TezID));
                command.Parameters.Add(command.CreateParameter("@TezDosyaYuklemeDuyuruID", tezDosyaYukleme.TezDosyaYuklemeDuyuruID));
                command.Parameters.Add(command.CreateParameter("@YuklenenOgrenciID", tezDosyaYukleme.YuklenenOgrenciID));
                command.Parameters.Add(command.CreateParameter("@DosyaAdi", tezDosyaYukleme.DosyaAdi));
                command.Parameters.Add(command.CreateParameter("@DosyaYolu", tezDosyaYukleme.DosyaYolu));
                command.Parameters.Add(command.CreateParameter("@DosyaAciklama", tezDosyaYukleme.DosyaAciklama));

                return ToList(command).FirstOrDefault();
            }
        }

        public int TezDosyaYuklemeSil(TezDosyaYukleme tezDosyaYukleme)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = SQLQueryList.TezDosyaYuklemeSil;

                command.Parameters.Add(command.CreateParameter("@ID", tezDosyaYukleme.ID));

                return Execute(command);
            }
        }
    }
}

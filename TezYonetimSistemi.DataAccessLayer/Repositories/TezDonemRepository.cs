using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TezYonetimSistemi.DataAccessLayer.Helpers;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.DataAccessLayer.Repositories
{
    public class TezDonemRepository:Repository<TezDonemi>
    {
        private DbContext _context;
        public TezDonemRepository(DbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public IList<TezDonemi> TezDonemiGetir()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandText = SQLQueryList.TezDonemiGetir;

                return ToList(command).ToList();
            }
        }
    }
}

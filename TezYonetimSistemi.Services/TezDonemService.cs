using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TezYonetimSistemi.DataAccessLayer;
using TezYonetimSistemi.DataAccessLayer.Repositories;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.Services
{
    public class TezDonemService : BaseService
    {
        public IList<TezDonemi> TezDonemiGetir()
        {
            using (var context = new DbContext(connectionFactory))
            {
                var TezDonemiRepo = new TezDonemRepository(context);
                return TezDonemiRepo.TezDonemiGetir();
            }
        }
    }
}

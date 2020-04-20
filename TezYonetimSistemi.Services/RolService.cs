using System.Collections.Generic;
using TezYonetimSistemi.DataAccessLayer;
using TezYonetimSistemi.DataAccessLayer.Repositories;
using TezYonetimSistemi.Model;

namespace TezYonetimSistemi.Services
{
    public class RolService : BaseService
    {
        public Rol RolEkle(Rol rol)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var RolRepo = new RolRepository(context);
                return RolRepo.RolEkle(rol);
            }
        }

        public Rol RolGuncelle(Rol rol)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var RolRepo = new RolRepository(context);
                return RolRepo.RolGuncelle(rol);
            }
        }

        public IList<Rol> RolleriGetir()
        {
            using (var context = new DbContext(connectionFactory))
            {
                var RolRepo = new RolRepository(context);
                return RolRepo.RolleriGetir();
            }
        }

        public int RolSil(Rol rol)
        {
            using (var context = new DbContext(connectionFactory))
            {
                var RolRepo = new RolRepository(context);
                return RolRepo.RolSil(rol);
            }
        }
    }
}

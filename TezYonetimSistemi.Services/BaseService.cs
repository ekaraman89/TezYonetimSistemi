using TezYonetimSistemi.DataAccessLayer;

namespace TezYonetimSistemi.Services
{
    public abstract class BaseService
    {
        public IConnectionFactory connectionFactory = new DbConnectionFactory("config");
    }
}

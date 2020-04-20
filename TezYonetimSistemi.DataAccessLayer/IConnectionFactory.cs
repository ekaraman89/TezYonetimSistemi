using System.Data;

namespace TezYonetimSistemi.DataAccessLayer
{
    /// <summary>
    /// Veritabanı oluşturacak olan sınıfların metodları burada...
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Bu sınıftan türetilen sınıflarda Create metodu olmak zorunda...
        /// </summary>
        /// <returns></returns>
        IDbConnection Create();
    }
}

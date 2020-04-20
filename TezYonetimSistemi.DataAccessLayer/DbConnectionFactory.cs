using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace TezYonetimSistemi.DataAccessLayer
{
    /// <summary>
    /// Veritabanına bağlantıyı yapan sınıf burası
    /// </summary>
  public  class DbConnectionFactory : IConnectionFactory
    {
        private readonly DbProviderFactory _provider;
        private readonly string _connectionString;
        private readonly string _name;

        /// <summary>
        /// connectionName verilerek yeni bir örnek oluşturulur.
        /// </summary>
        /// <param name="connectionName"></param>
        public DbConnectionFactory(string connectionName)
        {
            // connectionName boş gelirse hata fırlatır.... 
            if (string.IsNullOrWhiteSpace(connectionName)) throw new ArgumentNullException("connectionName");

            // Verilen connectionName web.config den çekilir
            var conStr = ConfigurationManager.ConnectionStrings[connectionName];

            // Eğer boş ise hata fırlatılır.
            if (conStr == null)
                throw new ConfigurationErrorsException($"web.config dosyasında {connectionName} adında bir tanımlama bulunamadı...");

            // ConnectionStringteki bilgiler doğrultusunda connection nesnesi hazırlanır. MSSQL-Oracle vs gibi 
            _name = conStr.ProviderName;
            _provider = DbProviderFactories.GetFactory(conStr.ProviderName);
            _connectionString = conStr.ConnectionString;
        }

        /// <summary>
        /// Veritabanı bağlantısı yapılıp geri gönderilir.
        /// </summary>
        /// <returns></returns>
        public IDbConnection Create()
        {
            // Hazırlanan provider ile ilgili connoction (MSSQL - Oracle vs) oluşturulur...
            var connection = _provider.CreateConnection();
            if (connection == null)
                throw new ConfigurationErrorsException($"web.config dosyasında {_name} adındaki tanımlama ile bağlantı oluşturulamadı...");

            connection.ConnectionString = _connectionString;
            connection.Open();
            return connection;
        }
    }
}

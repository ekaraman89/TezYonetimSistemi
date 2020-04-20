using System.Data;

namespace TezYonetimSistemi.DataAccessLayer.Extensions
{
    /// <summary>
    /// Command Nesnesi için yardımcı sınıf
    /// </summary>
    public static class IDbCommandExtensions
    {
        /// <summary>
        /// Command nesnesine parametre eklemek için kullanılır
        /// </summary>
        /// <param name="command">ilgili command</param>
        /// <param name="name">Parametre Adı</param>
        /// <param name="value">Parametre Değeri</param>
        /// <returns></returns>
        public static IDbDataParameter CreateParameter(this IDbCommand command, string name, object value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;

            return parameter;
        }
    }
}

using System;
using System.Data;

namespace TezYonetimSistemi.DataAccessLayer.Extensions
{
    public static class IDataRecordExtensions
    {
        /// <summary>
        /// Kaydın kolonlarına bakar, aranan kolon adını olup olmadığını geri döner
        /// </summary>
        /// <param name="dr">Tablonun bir satırı</param>
        /// <param name="columnName">Kolon adı</param>
        /// <returns></returns>
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record"></param>
        /// <returns></returns>
        public static T MapType<T>(this IDataRecord record) where T : class, new()
        {
            var objT = Activator.CreateInstance<T>();
            foreach (var property in typeof(T).GetProperties())
            {
                if (record.HasColumn(property.Name) && !record.IsDBNull(record.GetOrdinal(property.Name)))
                    property.SetValue(objT, record[property.Name]);
            }
            return objT;
        }
    }
}

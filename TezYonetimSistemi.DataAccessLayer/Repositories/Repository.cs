using System;
using System.Collections.Generic;
using System.Data;
using TezYonetimSistemi.DataAccessLayer.Extensions;

namespace TezYonetimSistemi.DataAccessLayer.Repositories
{
    /// <summary>
    /// BaseRepository new yapılabilen sınıftan türetilebilir...
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class Repository<TEntity> where TEntity : new()
    {
        DbContext _context;

        public Repository(DbContext dbContext)
        {
            _context = dbContext;
        }

        protected DbContext Context
        {
            get
            {
                return this._context;
            }
        }

        /// <summary>
        /// Veritabanından liste döndürür.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        protected IEnumerable<TEntity> ToList(IDbCommand command)
        {
            using (var record = command.ExecuteReader())
            {
                List<TEntity> items = new List<TEntity>();
                while (record.Read())
                {

                    items.Add(Map<TEntity>(record));
                }
                return items;
            }
        }


        protected int Execute(IDbCommand command)
        {
           return command.ExecuteNonQuery();
        }

        /// <summary>
        /// Veritabanından çekilen her bir kaydın, kolon adlarına göre nesnenin özelliklerine atama işlemi yapılır
        /// </summary>
        /// <typeparam name="TEntity">Gelen Nesne</typeparam>
        /// <param name="record">Gelen datanın tüm kolon adları</param>
        /// <returns>Veritabanında çekilen kaydı nesne olarak geriye döndürür</returns>
        protected TEntity Map<TEntity>(IDataRecord record)
        {
            var objT = Activator.CreateInstance<TEntity>();
            foreach (var property in typeof(TEntity).GetProperties())
            {
                if (record.HasColumn(property.Name) && !record.IsDBNull(record.GetOrdinal(property.Name)))
                    property.SetValue(objT, record[property.Name]);
            }
            return objT;
        }
    }
}

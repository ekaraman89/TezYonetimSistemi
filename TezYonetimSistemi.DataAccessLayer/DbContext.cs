using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace TezYonetimSistemi.DataAccessLayer
{
    /// <summary>
    /// Ilgili veritabanı nesnesini oluşturur...
    /// </summary>
    public class DbContext: IDisposable
    {
        private readonly IDbConnection _connection;
        private readonly IConnectionFactory _connectionFactory;
        private readonly ReaderWriterLockSlim _rwLock = new ReaderWriterLockSlim();
        private readonly LinkedList<AdoNetUnitOfWork> _uows = new LinkedList<AdoNetUnitOfWork>();

        /// <summary>
        /// DbContext sınıfı IConnectionFactory den bir nesne ile üretilir. Bu nesne doğrultusunda DbContex şekillenir.
        /// </summary>
        /// <param name="connectionFactory"></param>
        public DbContext(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _connection = connectionFactory.Create();
        }

        /// <summary>
        /// Transaction tanımlanır...
        /// Amaç: Herhangi bir hata olduğunda işlemlerin yarım kalmaması.
        /// </summary>
        /// <returns></returns>
        public IUnitOfWork CreateUnitOfWork()
        {
            var transaction = _connection.BeginTransaction();
            var uow = new AdoNetUnitOfWork(transaction, RemoveTransaction, RemoveTransaction);

            _rwLock.EnterWriteLock();
            _uows.AddLast(uow);
            _rwLock.ExitWriteLock();

            return uow;
        }

        public IDbCommand CreateCommand()
        {
            var cmd = _connection.CreateCommand();

            _rwLock.EnterReadLock();
            if (_uows.Count > 0)
                cmd.Transaction = _uows.First.Value.Transaction;
            _rwLock.ExitReadLock();

            return cmd;
        }

        /// <summary>
        /// İşlem herhangi bir sebepten dolayı hata alır ise, o ana kadar yapılan değişiklikleri geri alır.
        /// Örnek : Kullanıcı ekleyip, sonra kullanıcı id ile öğrenci eklerken. Kullanıcıyı ekledikten sonra, öğrenciyi ekleyemez ise, kullanıcıyı ekleme işlemini geriye alır.
        /// </summary>
        /// <param name="obj"></param>
        private void RemoveTransaction(AdoNetUnitOfWork obj)
        {
            _rwLock.EnterWriteLock();
            _uows.Remove(obj);
            _rwLock.ExitWriteLock();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}

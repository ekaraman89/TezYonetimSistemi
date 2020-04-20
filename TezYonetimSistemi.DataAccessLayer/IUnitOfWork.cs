namespace TezYonetimSistemi.DataAccessLayer
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// Bağalntıyı sonlardır...
        /// </summary>
        void Dispose();

        /// <summary>
        /// Tüm işlemler başarılıysa kaydet
        /// </summary>
        void SaveChanges();
    }
}

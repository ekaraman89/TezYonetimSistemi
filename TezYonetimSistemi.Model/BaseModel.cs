using System;
namespace TezYonetimSistemi.Model
{
    public abstract class BaseModel
    {
        public int ID { get; set; }
        public DateTime OlusturmaTarihi { get; set; }
    }
}

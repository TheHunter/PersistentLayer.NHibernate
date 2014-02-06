using System.Collections.Generic;
using PersistentLayer.Domain;

namespace PersistentLayer.NHibernate.Test.PocoPrj
{
    /// <summary>
    /// 
    /// </summary>
    public class SalesmanDetails
        : SalesmanPrj
    {
        public ICollection<TradeContract> OwnContracts { get; set; }
    }
}

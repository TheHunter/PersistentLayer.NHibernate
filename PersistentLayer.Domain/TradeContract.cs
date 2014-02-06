using System;
using EntityModel;

namespace PersistentLayer.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TradeContract
        : VersionedEntity<long?>
    {
        private long? number = null;
        private string description = null;
        private Salesman owner = null;
        private DateTime? beginDate = null;
        private int? price;

        public virtual long? Number
        {
            set { this.number = value; }
            get { return this.number; }
        }

        public virtual string Description
        {
            set { this.description = value; }
            get { return this.description; }
        }

        public virtual Salesman Owner
        {
            set { this.owner = value; }
            get { return this.owner; }
        }

        public virtual DateTime? BeginDate
        {
            set { this.beginDate = value; }
            get { return this.beginDate; }
        }


        public virtual int? Price
        {
            get { return price; }
            set { this.price = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract double ComputeReward();

    }
}

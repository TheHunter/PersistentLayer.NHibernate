using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersistentLayer.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class CarContract
        : TradeContract
    {
        private string brandName;

        /// <summary>
        /// 
        /// </summary>
        public virtual string BrandName
        {
            get { return this.brandName; }
            set { this.brandName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override double ComputeReward()
        {
            if (this.Price.HasValue)
                return this.Price.Value * 0.1;
            
            return 0;
        }
    }
}

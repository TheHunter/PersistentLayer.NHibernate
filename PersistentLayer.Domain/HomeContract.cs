using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersistentLayer.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeContract
        : TradeContract
    {
        private string town;

        /// <summary>
        /// 
        /// </summary>
        public virtual string Town
        {
            get { return this.town; }
            set { this.town = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override double ComputeReward()
        {
            if (this.Price.HasValue)
                return this.Price.Value * 0.01;

            return 0;
        }
    }
}

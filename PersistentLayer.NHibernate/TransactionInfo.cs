using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersistentLayer.Exceptions;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    public class TransactionInfo
        : ITransactionInfo
    {

        private readonly string name;
        private readonly int index;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="index"></param>
        internal protected TransactionInfo(string name, int index)
        {
            if (name == null || name.Trim().Equals(string.Empty))
                throw new BusinessLayerException("The name of this transaction info cannot be null or empty.", "TransactionInfo .ctor");

            if (index < 0)
                throw new BusinessLayerException("Index of the current Transactioninfo cannot be less than zero.");

            this.name = name;
            this.index = index;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Index
        {
            get { return this.index; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is TransactionInfo)
                return this.GetHashCode() == obj.GetHashCode();

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Name.GetHashCode() - this.Index;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Index: {0}, Name: {1}", this.Index, this.Name);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Type;

namespace PersistentLayer.NHibernate.Impl
{
    /// <summary>
    /// 
    /// </summary>
    internal class HqlBuilder
        : IHqlBuilder
    {

        private readonly IQuery rowCountQuery;
        private readonly IQuery hqlQuery;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hqlQuery"></param>
        internal protected HqlBuilder(IQuery hqlQuery)
        {
            this.hqlQuery = hqlQuery;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        public IHqlBuilder SetReadOnly(bool readOnly)
        {
            this.hqlQuery.SetReadOnly(readOnly);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheable"></param>
        /// <returns></returns>
        public IHqlBuilder SetCacheable(bool cacheable)
        {
            this.hqlQuery.SetCacheable(cacheable);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheRegion"></param>
        /// <returns></returns>
        public IHqlBuilder SetCacheRegion(string cacheRegion)
        {
            this.hqlQuery.SetCacheRegion(cacheRegion);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public IHqlBuilder SetTimeout(int timeout)
        {
            this.hqlQuery.SetTimeout(timeout);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fetchSize"></param>
        /// <returns></returns>
        public IHqlBuilder SetFetchSize(int fetchSize)
        {
            this.hqlQuery.SetFetchSize(fetchSize);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="lockMode"></param>
        /// <returns></returns>
        public IHqlBuilder SetLockMode(string alias, LockMode lockMode)
        {
            this.hqlQuery.SetLockMode(alias, lockMode);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public IHqlBuilder SetComment(string comment)
        {
            this.hqlQuery.SetComment(comment);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flushMode"></param>
        /// <returns></returns>
        public IHqlBuilder SetFlushMode(FlushMode flushMode)
        {
            this.hqlQuery.SetFlushMode(flushMode);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheMode"></param>
        /// <returns></returns>
        public IHqlBuilder SetCacheMode(CacheMode cacheMode)
        {
            this.hqlQuery.SetCacheMode(cacheMode);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IHqlBuilder SetParameter(int position, object val, IType type)
        {
            this.hqlQuery.SetParameter(position, val, type);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IHqlBuilder SetParameter(string name, object val, IType type)
        {
            this.hqlQuery.SetParameter(name, val, type);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetParameter<T>(int position, T val)
        {
            this.hqlQuery.SetParameter(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetParameter<T>(string name, T val)
        {
            this.hqlQuery.SetParameter(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetParameter(int position, object val)
        {
            this.hqlQuery.SetParameter(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetParameter(string name, object val)
        {
            this.hqlQuery.SetParameter(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="vals"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IHqlBuilder SetParameterList(string name, IEnumerable vals, IType type)
        {
            this.hqlQuery.SetParameter(name, vals, type);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        public IHqlBuilder SetParameterList(string name, IEnumerable vals)
        {
            this.hqlQuery.SetParameter(name, vals);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IHqlBuilder SetProperties(object obj)
        {
            this.hqlQuery.SetProperties(obj);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetAnsiString(int position, string val)
        {
            this.hqlQuery.SetAnsiString(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetAnsiString(string name, string val)
        {
            this.hqlQuery.SetAnsiString(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetBinary(int position, byte[] val)
        {
            this.hqlQuery.SetBinary(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetBinary(string name, byte[] val)
        {
            this.hqlQuery.SetBinary(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetBoolean(int position, bool val)
        {
            this.hqlQuery.SetBoolean(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetBoolean(string name, bool val)
        {
            this.hqlQuery.SetBoolean(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetByte(int position, byte val)
        {
            this.hqlQuery.SetByte(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetByte(string name, byte val)
        {
            this.hqlQuery.SetByte(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetCharacter(int position, char val)
        {
            this.hqlQuery.SetCharacter(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetCharacter(string name, char val)
        {
            this.hqlQuery.SetCharacter(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetDateTime(int position, DateTime val)
        {
            this.hqlQuery.SetDateTime(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetDateTime(string name, DateTime val)
        {
            this.hqlQuery.SetDateTime(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetDateTime2(int position, DateTime val)
        {
            this.hqlQuery.SetDateTime2(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetDateTime2(string name, DateTime val)
        {
            this.hqlQuery.SetDateTime2(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetTimeSpan(int position, TimeSpan val)
        {
            this.hqlQuery.SetTimeSpan(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetTimeSpan(string name, TimeSpan val)
        {
            this.hqlQuery.SetTimeSpan(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetTimeAsTimeSpan(int position, TimeSpan val)
        {
            this.hqlQuery.SetTimeSpan(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetTimeAsTimeSpan(string name, TimeSpan val)
        {
            this.hqlQuery.SetTimeAsTimeSpan(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetDateTimeOffset(int position, DateTimeOffset val)
        {
            this.hqlQuery.SetDateTimeOffset(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetDateTimeOffset(string name, DateTimeOffset val)
        {
            this.hqlQuery.SetDateTimeOffset(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetDecimal(int position, decimal val)
        {
            this.hqlQuery.SetDecimal(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetDecimal(string name, decimal val)
        {
            this.hqlQuery.SetDecimal(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetDouble(int position, double val)
        {
            this.hqlQuery.SetDouble(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetDouble(string name, double val)
        {
            this.hqlQuery.SetDouble(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetEnum(int position, Enum val)
        {
            this.hqlQuery.SetEnum(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetEnum(string name, Enum val)
        {
            this.hqlQuery.SetEnum(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetInt16(int position, short val)
        {
            this.hqlQuery.SetInt16(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetInt16(string name, short val)
        {
            this.hqlQuery.SetInt16(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetInt32(int position, int val)
        {
            this.hqlQuery.SetInt32(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetInt32(string name, int val)
        {
            this.hqlQuery.SetInt32(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetInt64(int position, long val)
        {
            this.hqlQuery.SetInt64(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetInt64(string name, long val)
        {
            this.hqlQuery.SetInt64(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetSingle(int position, float val)
        {
            this.hqlQuery.SetSingle(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetSingle(string name, float val)
        {
            this.hqlQuery.SetSingle(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetString(int position, string val)
        {
            this.hqlQuery.SetString(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetString(string name, string val)
        {
            this.hqlQuery.SetString(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetTime(int position, DateTime val)
        {
            this.hqlQuery.SetTime(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetTime(string name, DateTime val)
        {
            this.hqlQuery.SetTime(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetTimestamp(int position, DateTime val)
        {
            this.hqlQuery.SetTimestamp(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetTimestamp(string name, DateTime val)
        {
            this.hqlQuery.SetTimestamp(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetGuid(int position, Guid val)
        {
            this.hqlQuery.SetGuid(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetGuid(string name, Guid val)
        {
            this.hqlQuery.SetGuid(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetEntity(int position, object val)
        {
            this.hqlQuery.SetEntity(position, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public IHqlBuilder SetEntity(string name, object val)
        {
            this.hqlQuery.SetEntity(name, val);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public string QueryString
        {
            get { return hqlQuery.QueryString; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IType[] ReturnTypes
        {
            get { return hqlQuery.ReturnTypes; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] ReturnAliases
        {
            get { return hqlQuery.ReturnAliases; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string[] NamedParameters
        {
            get { return hqlQuery.NamedParameters; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return hqlQuery.IsReadOnly; }
        }
    }
}

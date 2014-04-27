using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Transform;
using NHibernate.Type;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHqlBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="readOnly"></param>
        /// <returns></returns>
        IHqlBuilder SetReadOnly(bool readOnly);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheable"></param>
        /// <returns></returns>
        IHqlBuilder SetCacheable(bool cacheable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheRegion"></param>
        /// <returns></returns>
        IHqlBuilder SetCacheRegion(string cacheRegion);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        IHqlBuilder SetTimeout(int timeout);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fetchSize"></param>
        /// <returns></returns>
        IHqlBuilder SetFetchSize(int fetchSize);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="alias"></param>
        /// <param name="lockMode"></param>
        /// <returns></returns>
        IHqlBuilder SetLockMode(string alias, LockMode lockMode);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        IHqlBuilder SetComment(string comment);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="flushMode"></param>
        /// <returns></returns>
        IHqlBuilder SetFlushMode(FlushMode flushMode);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheMode"></param>
        /// <returns></returns>
        IHqlBuilder SetCacheMode(CacheMode cacheMode);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IHqlBuilder SetParameter(int position, object val, IType type);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IHqlBuilder SetParameter(string name, object val, IType type);
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetParameter<T>(int position, T val);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetParameter<T>(string name, T val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetParameter(int position, object val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetParameter(string name, object val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="vals"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IHqlBuilder SetParameterList(string name, IEnumerable vals, IType type);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="vals"></param>
        /// <returns></returns>
        IHqlBuilder SetParameterList(string name, IEnumerable vals);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        IHqlBuilder SetProperties(object obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetAnsiString(int position, string val);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetAnsiString(string name, string val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetBinary(int position, byte[] val);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetBinary(string name, byte[] val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetBoolean(int position, bool val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetBoolean(string name, bool val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetByte(int position, byte val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetByte(string name, byte val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetCharacter(int position, char val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetCharacter(string name, char val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetDateTime(int position, DateTime val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetDateTime(string name, DateTime val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetDateTime2(int position, DateTime val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetDateTime2(string name, DateTime val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetTimeSpan(int position, TimeSpan val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetTimeSpan(string name, TimeSpan val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetTimeAsTimeSpan(int position, TimeSpan val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetTimeAsTimeSpan(string name, TimeSpan val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetDateTimeOffset(int position, DateTimeOffset val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetDateTimeOffset(string name, DateTimeOffset val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetDecimal(int position, decimal val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetDecimal(string name, decimal val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetDouble(int position, double val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetDouble(string name, double val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetEnum(int position, Enum val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetEnum(string name, Enum val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetInt16(int position, short val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetInt16(string name, short val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetInt32(int position, int val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetInt32(string name, int val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetInt64(int position, long val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetInt64(string name, long val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetSingle(int position, float val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetSingle(string name, float val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetString(int position, string val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetString(string name, string val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetTime(int position, DateTime val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetTime(string name, DateTime val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetTimestamp(int position, DateTime val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetTimestamp(string name, DateTime val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetGuid(int position, Guid val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetGuid(string name, Guid val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetEntity(int position, object val);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        IHqlBuilder SetEntity(string name, object val);

        /// <summary>
        /// 
        /// </summary>
        string QueryString { get; }
        
        /// <summary>
        /// 
        /// </summary>
        IType[] ReturnTypes { get; }
        
        /// <summary>
        /// 
        /// </summary>
        string[] ReturnAliases { get; }
        
        /// <summary>
        /// 
        /// </summary>
        string[] NamedParameters { get; }
        
        /// <summary>
        /// 
        /// </summary>
        bool IsReadOnly { get; }


        //IHqlBuilder SetMaxResults(int maxResults);
        //IHqlBuilder SetFirstResult(int firstResult);
        //IEnumerable Enumerable();
        //IEnumerable<T> Enumerable<T>();
        //IList List();
        //void List(IList results);
        //IList<T> List<T>();
        //object UniqueResult();
        //T UniqueResult<T>();
        //int ExecuteUpdate();

        //IHqlBuilder SetResultTransformer(IResultTransformer resultTransformer);
        //IEnumerable<T> Future<T>();
        //IFutureValue<T> FutureValue<T>();
        
    }
}

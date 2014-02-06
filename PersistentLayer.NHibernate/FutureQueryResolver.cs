using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using NHibernate;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    public static class FutureQueryResolver
    {
        private static readonly HashSet<FutureFunction> Functions;

        /// <summary>
        /// 
        /// </summary>
        static FutureQueryResolver()
        {
            Functions = new HashSet<FutureFunction>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public static IEnumerable ToFuture(ICriteria criteria)
        {
            Type collectionType;
            try
            {
                collectionType = criteria.GetRootEntityTypeIfAvailable() ?? typeof(object);
            }
            catch (Exception)
            {
                collectionType = typeof(object);
            }

            FutureFunction current = Functions.FirstOrDefault(n => n.CollectionType == collectionType);
            if (current == null)
            {
                current = new FutureFunction(collectionType);
                Functions.Add(current);
            }

            return current.ExecuteFuture(criteria);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class FutureFunction
    {
        private static readonly Type CriteriaType;
        private static readonly Type DefaultFunction;
        private static readonly MethodInfo CollectionMethod;
        
        private readonly MethodInfo futureMethod;
        private readonly Type collectionType;
        private readonly Func<ICriteria, IEnumerable> futureExecutor;

        /// <summary>
        /// 
        /// </summary>
        static FutureFunction()
        {
            CriteriaType = typeof(ICriteria);
            DefaultFunction = typeof(Func<ICriteria, IEnumerable>);
            CollectionMethod = CriteriaType.GetMethod("Future");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectionType"></param>
        public FutureFunction(Type collectionType)
        {
            this.collectionType = collectionType;
            futureMethod = CollectionMethod.MakeGenericMethod(collectionType);

            ParameterExpression parameter = Expression.Parameter(CriteriaType, "criteria");
            futureExecutor =
                Expression.Lambda<Func<ICriteria, IEnumerable>>(Expression.Call(parameter, futureMethod), parameter)
                          .Compile();
        }

        /// <summary>
        /// 
        /// </summary>
        public Type CollectionType
        {
            get { return this.collectionType; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IEnumerable ExecuteFuture(ICriteria criteria)
        {
            return this.futureExecutor.Invoke(criteria);
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

            if (obj is FutureFunction)
                return this.GetHashCode() == obj.GetHashCode();

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.collectionType.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Collection Future of <{0}>", this.collectionType.Name);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// Class FutureQueryResolver.
    /// </summary>
    public static class FutureQueryResolver
    {
        private static readonly HashSet<FutureFunction> Functions;

        /// <summary>
        /// Initializes static members of the <see cref="FutureQueryResolver"/> class.
        /// </summary>
        static FutureQueryResolver()
        {
            Functions = new HashSet<FutureFunction>();
        }

        /// <summary>
        /// To the future.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IEnumerable.</returns>
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
    /// Class FutureFunction.
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
        /// Initializes static members of the <see cref="FutureFunction"/> class.
        /// </summary>
        static FutureFunction()
        {
            CriteriaType = typeof(ICriteria);
            DefaultFunction = typeof(Func<ICriteria, IEnumerable>);
            CollectionMethod = CriteriaType.GetMethod("Future");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FutureFunction"/> class.
        /// </summary>
        /// <param name="collectionType">Type of the collection.</param>
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
        /// Gets the type of the collection.
        /// </summary>
        /// <value>The type of the collection.</value>
        public Type CollectionType
        {
            get { return this.collectionType; }
        }

        /// <summary>
        /// Executes the future.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>IEnumerable.</returns>
        public IEnumerable ExecuteFuture(ICriteria criteria)
        {
            return this.futureExecutor.Invoke(criteria);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is FutureFunction)
                return this.GetHashCode() == obj.GetHashCode();

            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return this.collectionType.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format("Collection Future of <{0}>", this.collectionType.Name);
        }
    }
}

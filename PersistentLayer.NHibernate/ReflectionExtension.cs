using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace PersistentLayer.NHibernate
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReflectionExtension
    {
        private static readonly HashSet<Type> NumericTypes;
        private static readonly HashSet<Type> NullableNumericTypes;
        private static readonly HashSet<Type> FloatingNumericTypes;
        private static readonly Type QueryableType;
        private static readonly Dictionary<Type, Delegate> ToListDelegates;
        private static readonly Dictionary<Type, Delegate> AsQueryableDelegates;
        
        private static readonly MethodInfo ToListDefMethod;
        private static readonly MethodInfo AsQueryableDefMethod;

        static ReflectionExtension()
        {
            QueryableType = typeof (Queryable);
            ToListDefMethod = typeof (Enumerable).GetMethod("ToList");
            AsQueryableDefMethod = typeof (Queryable).GetMethods()
                                                     .Single(info => info.Name == "AsQueryable" && info.IsGenericMethod);

            NumericTypes = new HashSet<Type>
                {
                    typeof(Byte),
                    typeof(SByte),
                    typeof(Int16),
                    typeof(UInt16),
                    typeof(Int32),
                    typeof(UInt32),
                    typeof(Int64),
                    typeof(UInt64),
                    typeof(Decimal),
                    typeof(Single),
                    typeof(Double)
                };

            NullableNumericTypes = new HashSet<Type>
                {
                    typeof(Byte?),
                    typeof(SByte?),
                    typeof(Int16?),
                    typeof(UInt16?),
                    typeof(Int32?),
                    typeof(UInt32?),
                    typeof(Int64?),
                    typeof(UInt64?),
                    typeof(Decimal?),
                    typeof(Single?),
                    typeof(Double?)
                };

            FloatingNumericTypes = new HashSet<Type>
                {
                    typeof(Single),
                    typeof(Double)
                };

            ToListDelegates = new Dictionary<Type, Delegate>();
            AsQueryableDelegates = new Dictionary<Type, Delegate>();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsFloatingNumericType(this Type type)
        {
            return FloatingNumericTypes.Contains(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
        {
            //return type.IsClass || type.IsInterface || type.IsAbstract || type.IsEquivalentTo()
            return type.IsClass || type.IsInterface || type.IsAbstract || type.GetInterface("Nullable`1", true) != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsReferenceType(this Type type)
        {
            return type.IsClass || type.IsInterface || type.IsAbstract;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAnonymous(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            // HACK: The only way to detect anonymous types right now.
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                && type.IsGenericType && type.Name.Contains("AnonymousType")
                && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                && (type.Attributes & TypeAttributes.NotPublic) == TypeAttributes.NotPublic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumeric(this Type type)
        {
            return NumericTypes.Contains(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableNumericType(this Type type)
        {
            return NullableNumericTypes.Contains(type);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool Implements(this Type type, string namingInterface)
        {
            return type.GetInterface(namingInterface, true) != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool Implements(this Type type, Type interfaceType)
        {
            return type.GetInterface(interfaceType.Name, true) != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableValueType(this Type type)
        {
            return type.GetInterface("Nullable`1", true) != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericArg"></param>
        /// <returns></returns>
        internal static Delegate ToListDelegate(Type genericArg)
        {
            if (ToListDelegates.ContainsKey(genericArg))
                return ToListDelegates[genericArg];

            MethodInfo method = ToListDefMethod.MakeGenericMethod(genericArg);

            Type inputType = typeof(IEnumerable<>).MakeGenericType(genericArg);
            Type returnType = typeof(List<>).MakeGenericType(genericArg);

            Delegate del = MakeDelegate(inputType, returnType, method);
            ToListDelegates.Add(genericArg, del);
            
            return del;
        }


        internal static Delegate AsQueryableDelegate(Type genericArg)
        {
            if (AsQueryableDelegates.ContainsKey(genericArg))
                return AsQueryableDelegates[genericArg];

            MethodInfo method = AsQueryableDefMethod.MakeGenericMethod(genericArg);

            Type inputType = typeof (IEnumerable<>).MakeGenericType(genericArg);
            Type returnType = typeof (IQueryable<>).MakeGenericType(genericArg);

            Delegate del = MakeDelegate(inputType, returnType, method);
            AsQueryableDelegates.Add(genericArg, del);

            return del;
        }


        internal static Delegate MakeDelegate(Type inputType, Type returnType, MethodInfo method)
        {
            Type functionType = typeof(Func<,>).MakeGenericType(inputType, returnType);
            Delegate del = Delegate.CreateDelegate(functionType, null, method);

            return del;
        }
    
    }
}

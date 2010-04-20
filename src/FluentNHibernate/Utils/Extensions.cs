using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Util;

namespace FluentNHibernate.Utils
{
    public static class Extensions
    {
        public static IEnumerable<T> ConcatSingle<T>(this IEnumerable<T> enumerable, T single)
        {
            return enumerable.Concat(new[] {single});
        }

        public static bool In<T>(this T instance, params T[] expected)
        {
            return expected.Any(x => instance.Equals(x));
        }

        public static bool Closes(this Type type, Type openGenericType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == openGenericType;
        }

        public static bool ClosesInterface(this Type type, Type openGenericInterface)
        {
            return type.IsGenericType && type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == openGenericInterface);
        }

        public static bool IsEnum(this Type type)
        {
            if (type.IsNullable())
                return type.GetGenericArguments()[0].IsEnum;

            return type.IsEnum;
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    
        public static T InstantiateUsingParameterlessConstructor<T>(this Type type)
        {
            return (T)type.InstantiateUsingParameterlessConstructor();
        }

        public static object InstantiateUsingParameterlessConstructor(this Type type)
        {
            var constructor = ReflectHelper.GetDefaultConstructor(type);

            if (constructor == null)
                throw new MissingConstructorException(type);

            return constructor.Invoke(null);
        }

        public static bool HasInterface(this Type type, Type interfaceType)
        {
            return type.GetInterfaces().Contains(interfaceType);
        }

        public static bool HasInterface<T>(this Type type)
        {
            return type.HasInterface(typeof(T));
        }
    }
}
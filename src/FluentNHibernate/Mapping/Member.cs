using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using FluentNHibernate.Utils;

namespace FluentNHibernate
{
    public abstract class Member : IEquatable<Member>
    {
        public abstract string Name { get; }
        public abstract Type PropertyType { get; }
        public abstract bool CanWrite { get; }
        public abstract MemberInfo MemberInfo { get; }
        public abstract Type DeclaringType { get; }
        public abstract bool HasIndexParameters { get; }
        public abstract bool IsMethod { get; }
        public abstract bool IsField { get; }
        public abstract bool IsProperty { get; }
        //   GetIndexParameters().Length == 0

        public bool Equals(Member other)
        {
            return other.MemberInfo.MetadataToken.Equals(MemberInfo.MetadataToken);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Member)) return false;
            return Equals((Member)obj);
        }

        public override int GetHashCode()
        {
            return MemberInfo.GetHashCode() ^ 3;
        }

        public static bool operator ==(Member left, Member right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Member left, Member right)
        {
            return !Equals(left, right);
        }

        public abstract void SetValue(object target, object value);
        public abstract object GetValue(object target);
    }

    internal class FieldMember : Member
    {
        private readonly FieldInfo _fieldInfo;

        public override void SetValue(object target, object value)
        {
            _fieldInfo.SetValue(target, value);
        }

        public override object GetValue(object target)
        {
            return _fieldInfo.GetValue(target);
        }

        public FieldMember(FieldInfo fieldInfo)
        {
            _fieldInfo = fieldInfo;
        }

        public override string Name
        {
            get { return _fieldInfo.Name; }
        }
        public override Type PropertyType
        {
            get { return _fieldInfo.FieldType; }
        }
        public override bool CanWrite
        {
            get { return true; }
        }
        public override MemberInfo MemberInfo
        {
            get { return _fieldInfo; }
        }
        public override Type DeclaringType
        {
            get { return _fieldInfo.DeclaringType; }
        }
        public override bool HasIndexParameters
        {
            get { return false; }
        }
        public override bool IsMethod
        {
            get { return false; }
        }
        public override bool IsField
        {
            get { return true; }
        }
        public override bool IsProperty
        {
            get { return false; }
        }
    }

    internal class MethodMember : Member
    {
        private readonly MethodInfo _methodInfo;

        public override void SetValue(object target, object value)
        {
            throw new NotSupportedException("Cannot set the value of a method Member.");
        }

        public override object GetValue(object target)
        {
            return _methodInfo.Invoke(target, null);
        }

        public MethodMember(MethodInfo propertyInfo)
        {
            _methodInfo = propertyInfo;
        }

        public override string Name
        {
            get { return _methodInfo.Name; }
        }
        public override Type PropertyType
        {
            get { return _methodInfo.ReturnType; }
        }
        public override bool CanWrite
        {
            get { return false; }
        }
        public override MemberInfo MemberInfo
        {
            get { return _methodInfo; }
        }
        public override Type DeclaringType
        {
            get { return _methodInfo.DeclaringType; }
        }
        public override bool HasIndexParameters
        {
            get { return false; }
        }
        public override bool IsMethod
        {
            get { return true; }
        }
        public override bool IsField
        {
            get { return false; }
        }
        public override bool IsProperty
        {
            get { return false; }
        }
    }

    internal class PropertyMember : Member
    {
        private readonly PropertyInfo _propertyInfo;

        public PropertyMember(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public override void SetValue(object target, object value)
        {
            _propertyInfo.SetValue(target, value, null);
        }

        public override object GetValue(object target)
        {
            return _propertyInfo.GetValue(target, null);
        }

        public override string Name
        {
            get { return _propertyInfo.Name; }
        }
        public override Type PropertyType
        {
            get { return _propertyInfo.PropertyType; }
        }
        public override bool CanWrite
        {
            get { return _propertyInfo.CanWrite; }
        }
        public override MemberInfo MemberInfo
        {
            get { return _propertyInfo; }
        }
        public override Type DeclaringType
        {
            get { return _propertyInfo.DeclaringType; }
        }
        public override bool HasIndexParameters
        {
            get { return _propertyInfo.GetIndexParameters().Length > 0; }
        }
        public override bool IsMethod
        {
            get { return false; }
        }
        public override bool IsField
        {
            get { return false; }
        }
        public override bool IsProperty
        {
            get { return true; }
        }
    }

    public static class MemberExtensions
    {
        public static Member ToMember(this PropertyInfo propertyInfo)
        {
            return new PropertyMember(propertyInfo);
        }

        public static Member ToMember(this MethodInfo methodInfo)
        {
            return new MethodMember(methodInfo);
        }

        public static Member ToMember(this FieldInfo fieldInfo)
        {
            return new FieldMember(fieldInfo);
        }

        public static Member ToMember(this MemberInfo memberInfo)
        {
            if (memberInfo is PropertyInfo)
                return ((PropertyInfo)memberInfo).ToMember();
            if (memberInfo is FieldInfo)
                return ((FieldInfo)memberInfo).ToMember();
            if (memberInfo is MethodInfo)
                return ((MethodInfo)memberInfo).ToMember();

            throw new InvalidOperationException("Cannot convert MemberInfo '" + memberInfo.Name + "' to Member.");
        }

        public static IEnumerable<Member> GetInstanceFields(this Type type)
        {
            foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                if (!field.Name.StartsWith("<"))
                    yield return field.ToMember();
        }

        public static IEnumerable<Member> GetInstanceMethods(this Type type)
        {
            foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                if (!method.Name.StartsWith("get_") && !method.Name.StartsWith("set_") && method.ReturnType != typeof(void) && method.GetParameters().Length == 0)
                    yield return method.ToMember();
        }

        public static IEnumerable<Member> GetInstanceProperties(this Type type)
        {
            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                yield return property.ToMember();
        }

        public static IEnumerable<Member> GetInstanceMembers(this Type type)
        {
            var members = new HashSet<Member>(new MemberEqualityComparer());

            type.GetInstanceProperties().Each(x => members.Add(x));
            type.GetInstanceFields().Each(x => members.Add(x));
            type.GetInstanceMethods().Each(x => members.Add(x));

            if (type.BaseType != typeof(object))
                type.BaseType.GetInstanceMembers().Each(x => members.Add(x));

            return members;
        }
    }

    public class MemberEqualityComparer : IEqualityComparer<Member>
    {
        public bool Equals(Member x, Member y)
        {
            return x.MemberInfo.MetadataToken.Equals(y.MemberInfo.MetadataToken);
        }

        public int GetHashCode(Member obj)
        {
            return obj.MemberInfo.MetadataToken.GetHashCode();
        }
    }
}

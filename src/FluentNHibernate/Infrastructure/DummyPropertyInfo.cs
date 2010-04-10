using System;
using System.Globalization;
using System.Reflection;

namespace FluentNHibernate.Infrastructure
{
    public sealed class DummyPropertyInfo : PropertyInfo
    {
        private readonly string name;
        private readonly Type type;

        public DummyPropertyInfo(string name, Type type)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (type == null) throw new ArgumentNullException("type");

            this.name = name;
            this.type = type;
        }

        public override int MetadataToken
        {
            get { return name.GetHashCode(); }
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotSupportedException("Can't use GetCustomAttributes in a DummyPropertyInfo");
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotSupportedException("Can't use IsDefined in a DummyPropertyInfo");
        }

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            throw new NotSupportedException("Can't use GetValue in a DummyPropertyInfo");
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
        {
            throw new NotSupportedException("Can't use SetValue in a DummyPropertyInfo");
        }

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            throw new NotSupportedException("Can't use GetAccessors in a DummyPropertyInfo");
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            return null;
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            return null;
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            throw new NotSupportedException("Can't use GetIndexParameters in a DummyPropertyInfo");
        }

        public override string Name
        {
            get { return name; }
        }

        public override Type DeclaringType
        {
            get { throw new NotSupportedException("Can't use DeclaringType in a DummyPropertyInfo"); }
        }

        public override Type ReflectedType
        {
            get { throw new NotSupportedException("Can't use ReflectedType in a DummyPropertyInfo"); }
        }

        public override Type PropertyType
        {
            get { return type; }
        }

        public override PropertyAttributes Attributes
        {
            get { throw new NotSupportedException("Can't use Attributes in a DummyPropertyInfo"); }
        }

        public override bool CanRead
        {
            get { throw new NotSupportedException("Can't use CanRead in a DummyPropertyInfo"); ; }
        }

        public override bool CanWrite
        {
            get { throw new NotSupportedException("Can't use CanWrite in a DummyPropertyInfo"); ; }
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotSupportedException("Can't use GetCustomAttributes in a DummyPropertyInfo");
        }
    }
}

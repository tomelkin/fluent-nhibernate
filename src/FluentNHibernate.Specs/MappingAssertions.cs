using System;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;
using FluentNHibernate.Utils;
using Machine.Specifications;

namespace FluentNHibernate.Specs
{
    public static class MappingAssertions
    {
        public static ClassMappingTester<T> For<T>(this ClassMapping mapping)
        {
            return new ClassMappingTester<T>(mapping);
        }

        public class IdMappingTester<T>
        {
            readonly IIdentityMapping mapping;

            public IdMappingTester(IIdentityMapping mapping)
            {
                this.mapping = mapping;
            }

            public IdMappingTester<T> Of<TMapping>()
            {
                if (!(mapping is TMapping))
                    throw new SpecificationException(string.Format("Should have id of {0} but was {1}.", typeof(TMapping).Name, mapping.GetType().Name));
                return this;
            }

            public IdMappingTester<T> ForMember(Member member)
            {
                if (mapping.As<IdMapping>().Member != member)
                    throw new SpecificationException(string.Format("Should have id with member of {0} but was {1}.", member.Name, mapping.As<IdMapping>().Name));
                return this;   
            }
        }

        public class PropertyMappingTester<T>
        {
            readonly PropertyMapping mapping;

            public PropertyMappingTester(PropertyMapping mapping)
            {
                this.mapping = mapping;
            }
        }

        public class ClassMappingTester<T>
        {
            readonly ClassMapping mapping;

            public ClassMappingTester(ClassMapping mapping)
            {
                this.mapping = mapping;
            }

            public IdMappingTester<T> ShouldHaveId()
            {
                if (mapping.Id == null)
                    throw new SpecificationException(string.Format("Should have id for {0} mapping but does not.", mapping.Type.Name));
                
                return new IdMappingTester<T>(mapping.Id);
            }

            public IdMappingTester<T> ShouldHaveId(Expression<Func<T, object>> exp)
            {
                return ShouldHaveId()
                    .Of<IdMapping>()
                    .ForMember(exp.ToMember());
            }

            public ClassMappingTester<T> ShouldHaveProperties()
            {
                if (!mapping.Properties.Any())
                    throw new SpecificationException(string.Format("Should have any properties for {0} mapping but does not.", mapping.Type.Name));

                return this;
            }

            public ClassMappingTester<T> ShouldntHaveAnyProperties()
            {
                if (mapping.Properties.Any())
                    throw new SpecificationException(string.Format("Should not have any properties for {0} mapping but does.", mapping.Type.Name));

                return this;
            }

            public PropertyMappingTester<T> ShouldHaveProperty(Expression<Func<T, object>> exp)
            {
                ShouldHaveProperties();

                var member = exp.ToMember();
                var propertyMapping = mapping.Properties.FirstOrDefault(x => x.Member == member);

                if (propertyMapping == null)
                    throw new SpecificationException(string.Format("Should have property for member {0} in {1} mapping but does not.", member.Name, mapping.Type.Name));

                return new PropertyMappingTester<T>(propertyMapping);
            }

            public ClassMappingTester<T> ShouldntHaveProperty(Expression<Func<T, object>> exp)
            {
                var member = exp.ToMember();
                var propertyMapping = mapping.Properties.FirstOrDefault(x => x.Member == member);

                if (propertyMapping != null)
                    throw new SpecificationException(string.Format("Should not have property for member {0} in {1} mapping but does.", member.Name, mapping.Type.Name));

                return this;
            }

            public ClassMappingTester<T> ShouldHaveName(string name)
            {
                if (mapping.Name != name)
                    throw new SpecificationException(string.Format("Should have name {0} for {1} mapping but has {2}.", name, mapping.Type.Name, mapping.Name));

                return this;
            }

            public ClassMappingTester<T> ShouldHaveNameMatchingAssemblyQualifiedTypeName()
            {
                ShouldHaveName(typeof(T).AssemblyQualifiedName);

                return this;
            }
        }
    }
}
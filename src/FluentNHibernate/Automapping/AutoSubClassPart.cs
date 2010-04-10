using System;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
    [Obsolete("Use a SubclassMap<T> with AutoMap enabled.")]
    public class AutoSubClassPart<T> : SubClassPart<T>, IAutoClasslike
    {
        public AutoSubClassPart(DiscriminatorPart parent, object discriminatorValue)
            : base(parent, discriminatorValue)
        {}

        public IMappingAction GetAction()
        {
            throw new NotSupportedException("Obsolete");
        }

        public void DiscriminateSubClassesOnColumn(string column)
        {}

        public IAutoClasslike JoinedSubClass(Type type, string keyColumn)
        {
            throw new NotSupportedException("Obsolete");
        }

        public IAutoClasslike SubClass(Type type, string discriminatorValue)
        {
            throw new NotSupportedException("Obsolete");
        }

        public void AlterModel(ClassMappingBase mapping)
        {}
    }
}
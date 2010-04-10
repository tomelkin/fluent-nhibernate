using System;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.FluentInterface.Fixtures;

namespace FluentNHibernate.Specs.FluentInterface
{
    public abstract class ProviderSpec
    {
        public static ClassMapping map_as_class<T>(Action<ClassMap<T>> setup)
        {
            var provider = new ClassMap<T>();

            setup(provider);

            return provider.GetClassMapping();
        }

        public static SubclassMapping map_as_subclass<TParent, T>(Action<SubclassMap<T>> setup)
            where TParent : EntityParent
        {
            var parent = new ClassMap<TParent>();
            parent.Id(x => x.Id);
            var provider = new SubclassMap<T>();

            setup(provider);

            return provider.GetSubclassMapping(parent);
        }
    }

    public abstract class AutomapProviderSpec
    {
        public static ClassMapping map_as_class<T>()
        {
            return map_as_class<T>(m => {});
        }

        public static ClassMapping map_as_class<T>(Action<ClassMap<T>> setup)
        {
            var provider = new ClassMap<T>();

            provider.AutoMap.This();
            setup(provider);

            return provider.GetClassMapping();
        }
    }
}
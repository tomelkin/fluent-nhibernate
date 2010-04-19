using FluentNHibernate.Infrastructure;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.FluentInterface.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.FluentInterface.ClassMapSpecs
{
    public class when_class_map_is_told_to_automap
    {
        Establish context = () =>
            map = new ClassMap<EntityWithPrivateProperties>();

        Because of = () =>
        {
            map.AutoMap.This();
            action = map.As<IProvider>().GetAction();
        };

        It should_return_an_automap_action = () =>
            action.ShouldBeOfType<AutomapAction>();
        
        static ClassMap<EntityWithPrivateProperties> map;
        static IMappingAction action;
    }

    public class when_class_map_isnt_told_to_automap
    {
        Establish context = () =>
            map = new ClassMap<EntityWithPrivateProperties>();

        Because of = () =>
            action = map.As<IProvider>().GetAction();

        It should_return_a_manual_action = () =>
            action.ShouldBeOfType<ManualAction>();

        static ClassMap<EntityWithPrivateProperties> map;
        static IMappingAction action;
    }

    public class when_class_map_is_told_to_automap_everything : AutomapProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithProperties>();

        Behaves_like<ClasslikeIdBehaviour> an_id_in_a_classlike_mapping;
        Behaves_like<ClasslikePropertyBehaviour> a_property_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }
}
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.FluentInterface.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.FluentInterface.ClassMapSpecs
{
    public class when_class_map_is_told_to_map_an_id : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithProperties>(m => m.Id(x => x.Id));

        Behaves_like<ClasslikeIdBehaviour> an_id_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_an_id_field : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithFields>(m => m.Id(x => x.Id));

        Behaves_like<ClasslikeIdBehaviour> an_id_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }

    public class when_class_map_is_told_to_map_a_private_property_id_using_reveal : ProviderSpec
    {
        Because of = () =>
            mapping = map_as_class<EntityWithPrivateProperties>(m => m.Id(Reveal.Property<EntityWithPrivateProperties>("Id")));

        Behaves_like<ClasslikeIdBehaviour> an_id_in_a_classlike_mapping;

        protected static ClassMapping mapping;
    }
}
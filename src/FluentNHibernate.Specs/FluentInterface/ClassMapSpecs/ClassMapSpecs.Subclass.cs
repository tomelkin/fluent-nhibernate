using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.FluentInterface.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.FluentInterface.ClassMapSpecs
{
    public class when_class_map_is_told_to_create_an_inline_subclass : ProviderSpec
    {
        // ignored warning for obsolete SubClass
#pragma warning disable 612,618

        Because of = () =>
            class_mapping = map_as_class<SuperTarget>(m =>
            {
                m.Id(x => x.Id);
                m.DiscriminateSubClassesOnColumn("col")
                    .SubClass<ChildTarget>(sc => { });
            });

#pragma warning restore 612,618

        It should_add_subclass_to_class_mapping_subclasses_collection = () =>
            class_mapping.Subclasses.Count().ShouldEqual(1);

        static ClassMapping class_mapping;
    }
}
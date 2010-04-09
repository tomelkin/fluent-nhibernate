using System;
using System.Collections.Generic;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Automapping.V2.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Automapping.V2
{
    public class when_the_automapper_is_mapping_a_class
    {
        Establish context = () =>
            automapper = new AutomapperV2(new StubConventionFinder());

        Because of = () =>
            mapping = automapper.MapEntity<Entity>();

        It should_set_the_class_name_to_the_assembly_qualified_type_name= () =>
            mapping.For<Entity>()
                .ShouldHaveNameMatchingAssemblyQualifiedTypeName();

        static ClassMapping mapping;
        static AutomapperV2 automapper;

        class StubConventionFinder : IConventionFinder
        {
            public IEnumerable<T> Find<T>() where T : IConvention
            {
                return new T[0];
            }
        }
    }
}
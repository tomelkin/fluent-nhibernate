using FluentNHibernate.Infrastructure;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.Compiler.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Compiler
{
    public class when_the_mapping_compiler_is_told_to_compile_a_automap_action
        : MappingCompilerSpec
    {
        Establish context = () =>
            instructions.AddActions(new AutomapAction(new[] { Action.For<Entity>() }));

        Because of = () =>
            Catch.Exception(() => compiler.BuildMappings());

        It should_call_the_automap_compile = () =>
            compiler.AutoMapCalled.ShouldBeTrue();
    }

    public class when_the_mapping_compiler_is_told_to_compile_a_manual_action
        : MappingCompilerSpec
    {
        Establish context = () =>
            instructions.AddActions(new ManualAction(Mapping.For<Entity>()));

        Because of = () =>
            Catch.Exception(() => compiler.BuildMappings());

        It should_call_the_manual_compile = () =>
            compiler.ManualMapCalled.ShouldBeTrue();
    }

    public class when_the_mapping_compiler_is_compiling_an_automap_action_with_no_user_defined_mappings
        : MappingCompilerSpec
    {
        Establish context = () =>
            instructions.AddActions(new AutomapAction(new[] { Action.For<FullyAutomappedEntity>() }));

        Because of = () =>
            mapping = compiler.BuildMappingFor<FullyAutomappedEntity>();

        It should_map_an_id_for_the_entity_following_the_default_rules = () =>
            mapping.For<FullyAutomappedEntity>()
                .ShouldHaveId(x => x.Id);
    }
}

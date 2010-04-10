using FluentNHibernate.Automapping;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.Mapping;
using FluentNHibernate.Specs.Automapping.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Compiler
{
    public class when_a_partial_automap_action_is_told_to_create_an_automapping_target_with_an_empty_setup_and_empty_main_instructions
    {
        Establish context = () =>
            action = new PartialAutomapAction(typeof(Entity), new AutomappingEntitySetup());

        Because of = () =>
            target = action.CreateTarget(new AutomappingInstructions());

        It should_use_the_default_automapping_configuration = () =>
        {
            target.Instructions.ShouldNotBeNull();
            target.Instructions.Configuration.ShouldBeOfType<DefaultAutomappingConfiguration>();
        };

        static PartialAutomapAction action;
        static AutomappingTarget target;
    }

    public class when_a_partial_automap_action_is_told_to_create_an_automapping_target_with_an_empty_setup_and_a_config_in_the_main_instructions
    {
        Establish context = () =>
        {
            cfg = new DefaultAutomappingConfiguration();
            instructions = new AutomappingInstructions();
            instructions.UseConfiguration(cfg);
            action = new PartialAutomapAction(typeof(Entity), new AutomappingEntitySetup());
        };

        Because of = () =>
            target = action.CreateTarget(instructions);

        It should_use_the_configuration_from_the_main_instructions = () =>
            target.Instructions.Configuration.ShouldEqual(cfg);

        static PartialAutomapAction action;
        static AutomappingTarget target;
        static AutomappingInstructions instructions;
        static DefaultAutomappingConfiguration cfg;
    }

    public class when_a_partial_automap_action_is_told_to_create_an_automapping_target_with_a_setup_with_a_configuration
    {
        Establish context = () =>
        {
            cfg = new DefaultAutomappingConfiguration();
            action = new PartialAutomapAction(typeof(Entity), new AutomappingEntitySetup {Configuration = cfg});
        };

        Because of = () =>
            target = action.CreateTarget(new AutomappingInstructions());

        It should_use_the_configuration_from_the_main_instructions = () =>
            target.Instructions.Configuration.ShouldEqual(cfg);

        static PartialAutomapAction action;
        static AutomappingTarget target;
        static DefaultAutomappingConfiguration cfg;
    }

    public class when_a_partial_automap_action_is_told_to_create_an_automapping_target_with_a_setup_with_exclusions
    {
        Establish context = () =>
        {
            cfg = new DefaultAutomappingConfiguration();
            var setup = new AutomappingEntitySetup {Configuration = cfg};
            setup.AddExclusion(m => true);
            action = new PartialAutomapAction(typeof(Entity), setup);
        };

        Because of = () =>
            target = action.CreateTarget(new AutomappingInstructions());

        It should_use_the_configuration_from_the_main_instructions = () =>
            target.Instructions.Configuration.ShouldBeOfType<EntityAutomappingInstructions.ExclusionWrappedConfiguration>();

        It should_exclude_members_using_the_exclusions_in_the_configuration = () =>
            target.Instructions.Configuration.ShouldMap(FakeMembers.IListOfStrings).ShouldBeFalse();

        static PartialAutomapAction action;
        static AutomappingTarget target;
        static DefaultAutomappingConfiguration cfg;
    }
}
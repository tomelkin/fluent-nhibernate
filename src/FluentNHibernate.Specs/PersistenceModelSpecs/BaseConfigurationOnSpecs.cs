using System;
using System.Collections.Generic;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures;
using Machine.Specifications;
using NHibernate.Cfg;

namespace FluentNHibernate.Specs.PersistenceModelSpecs
{
    public class when_the_persistence_model_has_nothing_configured_other_than_a_base_model
    {
        Establish context = () =>
        {
            var baseConfiguration = new StubPersistenceModel
            {
                AutomappingInstructions = (baseAutomappingInstructions = new AutomappingInstructions()),
                Conventions = (baseConventions = new ConventionsCollection { new StubConvention() }),
                Database = (baseDatabase = new StubDatabaseConfiguration()),
                Actions = (baseActions = new[] { new StubAction() }),
                PostConfigure = (basePostConfigure = cfg => {}),
                PreConfigure = (basePreConfigure = cfg => {})
            };
            model = new PersistenceModelWithBaseConfiguration(baseConfiguration);
        };

        Because of = () =>
            instructions = model.GetInstructions();

        It should_use_the_base_model_automapping_instructions_in_the_instructions = () =>
            instructions.AutomappingInstructions.ShouldEqual(baseAutomappingInstructions);

        It should_use_the_base_model_conventions_in_the_instructions = () =>
            instructions.Conventions.ShouldEqual(baseConventions);

        It should_use_the_base_model_database_configuration_in_the_instructions = () =>
            instructions.Database.ShouldEqual(baseDatabase);

        It should_use_the_base_model_actions_in_the_instructions = () =>
            instructions.GetActions().ShouldEqual(baseActions);

        It should_use_the_base_model_post_configure_action_in_the_instructions = () =>
            instructions.PostConfigure.ShouldEqual(basePostConfigure);

        It should_use_the_base_model_pre_configure_action_in_the_instructions = () =>
            instructions.PreConfigure.ShouldEqual(basePreConfigure);

        static PersistenceModel model;
        static IPersistenceInstructions instructions;
        static IAutomappingInstructions baseAutomappingInstructions;
        static ConventionsCollection baseConventions;
        static IDatabaseConfiguration baseDatabase;
        static IEnumerable<IMappingAction> baseActions;
        static Action<Configuration> basePostConfigure;
        static Action<Configuration> basePreConfigure;
    }

    public class when_the_persistence_model_has_everything_configured_and_a_base_model
    {
        Establish context = () =>
        {
            var baseConfiguration = new StubPersistenceModel
            {
                AutomappingInstructions = (baseAutomappingInstructions = new AutomappingInstructions()),
                Conventions = (baseConventions = new ConventionsCollection { new StubConvention() }),
                Database = (baseDatabase = new StubDatabaseConfiguration()),
                Actions = (baseActions = new[] { new StubAction() }),
                PostConfigure = (basePostConfigure = cfg => { }),
                PreConfigure = (basePreConfigure = cfg => { })
            };
            model = new PersistenceModelWithBaseConfigurationAndEverythingSet(baseConfiguration);
        };

        Because of = () =>
            instructions = model.GetInstructions();

        It should_not_use_the_base_model_automapping_instructions_in_the_instructions = () =>
            instructions.AutomappingInstructions.ShouldNotEqual(baseAutomappingInstructions);

        It should_not_use_the_base_model_conventions_in_the_instructions = () =>
            instructions.Conventions.ShouldNotEqual(baseConventions);

        It should_not_use_the_base_model_database_configuration_in_the_instructions = () =>
            instructions.Database.ShouldNotEqual(baseDatabase);

        It should_not_use_the_base_model_actions_in_the_instructions = () =>
            instructions.GetActions().ShouldNotEqual(baseActions);

        It should_not_use_the_base_model_post_configure_action_in_the_instructions = () =>
            instructions.PostConfigure.ShouldNotEqual(basePostConfigure);

        It should_not_use_the_base_model_pre_configure_action_in_the_instructions = () =>
            instructions.PreConfigure.ShouldNotEqual(basePreConfigure);

        static PersistenceModel model;
        static IPersistenceInstructions instructions;
        static IAutomappingInstructions baseAutomappingInstructions;
        static ConventionsCollection baseConventions;
        static IDatabaseConfiguration baseDatabase;
        static IEnumerable<IMappingAction> baseActions;
        static Action<Configuration> basePostConfigure;
        static Action<Configuration> basePreConfigure;
    }

}
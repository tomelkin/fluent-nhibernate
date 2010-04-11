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
    public class when_the_persistence_model_has_everything_configured_and_an_extended_model
    {
        Establish context = () =>
        {
            var extendedConfiguration = new StubPersistenceModel
            {
                AutomappingInstructions = (extendedAutomappingInstructions = new AutomappingInstructions()),
                Conventions = (extendedConventions = new ConventionsCollection { new StubConvention() }),
                Database = (extendedDatabase = new StubDatabaseConfiguration()),
                Actions = (extendedActions = new[] { new StubAction() }),
                PostConfigure = (extendedPostConfigure = cfg => { }),
                PreConfigure = (extendedPreConfigure = cfg => { })
            };
            model = new PersistenceModelWithExtendedConfigurationAndEverythingSet(extendedConfiguration);
        };

        Because of = () =>
            instructions = model.GetInstructions();

        It should_use_the_extended_model_automapping_instructions_in_the_instructions = () =>
            instructions.AutomappingInstructions.ShouldEqual(extendedAutomappingInstructions);

        It should_use_the_extended_model_conventions_in_the_instructions = () =>
            instructions.Conventions.ShouldEqual(extendedConventions);

        It should_use_the_extended_model_database_configuration_in_the_instructions = () =>
            instructions.Database.ShouldEqual(extendedDatabase);

        It should_use_the_extended_model_actions_in_the_instructions = () =>
            instructions.GetActions().ShouldEqual(extendedActions);

        It should_use_the_extended_model_post_configure_action_in_the_instructions = () =>
            instructions.PostConfigure.ShouldEqual(extendedPostConfigure);

        It should_use_the_extended_model_pre_configure_action_in_the_instructions = () =>
            instructions.PreConfigure.ShouldEqual(extendedPreConfigure);

        static PersistenceModel model;
        static IPersistenceInstructions instructions;
        static IAutomappingInstructions extendedAutomappingInstructions;
        static ConventionsCollection extendedConventions;
        static IDatabaseConfiguration extendedDatabase;
        static IEnumerable<IMappingAction> extendedActions;
        static Action<Configuration> extendedPostConfigure;
        static Action<Configuration> extendedPreConfigure;
    }
}
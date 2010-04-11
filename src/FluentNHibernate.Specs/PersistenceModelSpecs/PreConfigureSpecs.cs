using System;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures;
using Machine.Specifications;
using NHibernate.Cfg;

namespace FluentNHibernate.Specs.PersistenceModelSpecs
{
    public class when_the_persistence_model_has_a_pre_configure_action_specified
    {
        Establish context = () =>
        {
            preConfigureAction = cfg => {};
            model = new PersistenceModelWithPreConfigure(preConfigureAction);
        };

        Because of = () =>
            instructions = model.GetInstructions();

        It should_include_the_action_in_the_instructions_it_creates = () =>
            instructions.PreConfigure.ShouldEqual(preConfigureAction);

        static PersistenceModel model;
        static IPersistenceInstructions instructions;
        static Action<Configuration> preConfigureAction;
    }

    public class when_persistence_instructions_with_a_pre_configure_action_is_applied_to_a_configuration
    {
        Establish context = () =>
        {
            instructions = new PersistenceInstructions();
            instructions.UsePreConfigureAction(x =>
            {
                preConfigureActionCalled = true;
            });
            cfg = new Configuration();
        };

        Because of = () =>
            instructions.Apply(cfg);

        It should_apply_the_pre_configure_action = () =>
            preConfigureActionCalled.ShouldBeTrue();

        static PersistenceInstructions instructions;
        static bool preConfigureActionCalled;
        static Configuration cfg;
    }
}

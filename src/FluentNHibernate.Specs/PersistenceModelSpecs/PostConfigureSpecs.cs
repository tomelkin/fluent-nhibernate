using System;
using System.Linq;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures;
using Machine.Specifications;
using NHibernate.Cfg;

namespace FluentNHibernate.Specs.PersistenceModelSpecs
{
    public class when_the_persistence_model_has_a_post_configure_action_specified
    {
        Establish context = () =>
        {
            postConfigureAction = cfg => { };
            model = new PersistenceModelWithPostConfigure(postConfigureAction);
        };

        Because of = () =>
            instructions = model.GetInstructions();

        It should_include_the_action_in_the_instructions_it_creates = () =>
            instructions.PostConfigure.ShouldEqual(postConfigureAction);

        static PersistenceModel model;
        static IPersistenceInstructions instructions;
        static Action<Configuration> postConfigureAction;
    }

    public class when_persistence_instructions_with_a_post_configure_action_is_applied_to_a_configuration
    {
        Establish context = () =>
        {
            instructions = new PersistenceInstructions();
            instructions.UsePostConfigureAction(x =>
            {
                postConfigureActionCalled = true;
            });
            cfg = new Configuration();
        };

        Because of = () =>
            instructions.Apply(cfg);

        It should_apply_the_post_configure_action = () =>
            postConfigureActionCalled.ShouldBeTrue();

        static PersistenceInstructions instructions;
        static bool postConfigureActionCalled;
        static Configuration cfg;
    }
}
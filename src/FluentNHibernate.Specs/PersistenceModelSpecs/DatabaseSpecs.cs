using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures;
using Machine.Specifications;
using NHibernate.Cfg;

namespace FluentNHibernate.Specs.PersistenceModelSpecs
{
    public class when_the_persistence_model_has_a_database_specified
    {
        Establish context = () =>
        {
            db = new StubDatabaseConfiguration();
            model = new PersistenceModelWithDatabase(db);
        };

        Because of = () =>
            instructions = model.GetInstructions();

        It should_include_the_database_configuration_in_the_instructions = () =>
            instructions.Database.ShouldEqual(db);

        static PersistenceModel model;
        static IPersistenceInstructions instructions;
        static IDatabaseConfiguration db;
    }

    public class when_the_persistence_model_has_a_database_specified_as_a_persistence_configurer
    {
        Establish context = () =>
        {
            persistenceConfigurer = new StubPersistenceConfigurer();
            model = new PersistenceModelWithDatabaseAsPersistenceConfigurer(persistenceConfigurer);
        };

        Because of = () =>
            instructions = model.GetInstructions();

        It should_wrap_the_persistence_configurer_in_the_instructions = () =>
        {
            instructions.Database.ShouldBeOfType<PreconfiguredDatabaseConfiguration>();
            instructions.Database.As<PreconfiguredDatabaseConfiguration>()
                .Configurer.ShouldEqual(persistenceConfigurer);
        };

        static PersistenceModel model;
        static IPersistenceInstructions instructions;
        static IPersistenceConfigurer persistenceConfigurer;
    }

    public class when_persistence_instructions_with_a_database_is_applied_to_a_configuration
    {
        Establish context = () =>
        {
            db = new MockDatabaseConfiguration();
            instructions = new PersistenceInstructions();
            instructions.UseDatabaseConfiguration(db);
            cfg = new Configuration();
        };

        Because of = () =>
            instructions.Apply(cfg);

        It should_apply_the_database_settings_to_the_configuration = () =>
            db.ConfigureCalled.ShouldBeTrue();

        static PersistenceInstructions instructions;
        static Configuration cfg;
        static MockDatabaseConfiguration db;
    }
}

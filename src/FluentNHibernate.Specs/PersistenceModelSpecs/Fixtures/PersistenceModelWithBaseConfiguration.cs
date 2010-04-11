using FluentNHibernate.Conventions;

namespace FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures
{
    class PersistenceModelWithBaseConfiguration : PersistenceModel
    {
        public PersistenceModelWithBaseConfiguration(IPersistenceModel baseConfiguration)
        {
            BaseConfigurationOn(baseConfiguration);
        }
    }

    class PersistenceModelWithBaseConfigurationAndEverythingSet : PersistenceModelWithBaseConfiguration
    {
        public PersistenceModelWithBaseConfigurationAndEverythingSet(IPersistenceModel baseConfiguration)
            : base(baseConfiguration)
        {
            AutoMap.ThisAssembly();
            Conventions.Add(new StubConvention());
            Database(new StubDatabaseConfiguration());
            PreConfigure(cfg => method());
            PostConfigure(cfg => method());
        }

        void method() {}

        class StubConvention : IConvention {}
    }
}
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;

namespace FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures
{
    class MockDatabaseConfiguration : IDatabaseConfiguration
    {
        public bool ConfigureCalled { get; private set; }

        public void Configure(Configuration cfg)
        {
            ConfigureCalled = true;
        }
    }
}
using System;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;

namespace FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures
{
    class StubDatabaseConfiguration : IDatabaseConfiguration
    {
        public void Configure(Configuration cfg)
        {}
    }
}
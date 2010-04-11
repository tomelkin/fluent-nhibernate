using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;

namespace FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures
{
    class StubPersistenceConfigurer : IPersistenceConfigurer
    {
        public Configuration ConfigureProperties(Configuration nhibernateConfig)
        {
            return nhibernateConfig;
        }
    }
}
using FluentNHibernate.Cfg.Db;

namespace FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures
{
    class PersistenceModelWithDatabaseAsPersistenceConfigurer : PersistenceModel
    {
        public PersistenceModelWithDatabaseAsPersistenceConfigurer(IPersistenceConfigurer persistenceConfigurer)
        {
            Database(persistenceConfigurer);
        }
    }
}
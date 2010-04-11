using FluentNHibernate.Cfg.Db;

namespace FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures
{
    class PersistenceModelWithDatabase : PersistenceModel
    {
        public PersistenceModelWithDatabase(IDatabaseConfiguration db)
        {
            Database(db);
        }
    }
}
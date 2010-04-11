using FluentNHibernate.Conventions;

namespace FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures
{
    class PersistenceModelWithEverythingSet : PersistenceModel
    {
        public PersistenceModelWithEverythingSet()
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
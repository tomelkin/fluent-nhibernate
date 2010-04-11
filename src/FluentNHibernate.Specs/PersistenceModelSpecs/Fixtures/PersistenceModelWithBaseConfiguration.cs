namespace FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures
{
    class PersistenceModelWithBaseConfiguration : PersistenceModel
    {
        public PersistenceModelWithBaseConfiguration(IPersistenceModel baseConfiguration)
        {
            BaseConfigurationOn(baseConfiguration);
        }
    }

    class PersistenceModelWithBaseConfigurationAndEverythingSet : PersistenceModelWithEverythingSet
    {
        public PersistenceModelWithBaseConfigurationAndEverythingSet(IPersistenceModel baseConfiguration)
        {
            BaseConfigurationOn(baseConfiguration);
        }
    }
}
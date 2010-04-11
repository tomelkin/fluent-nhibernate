namespace FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures
{
    class PersistenceModelWithExtendedConfigurationAndEverythingSet : PersistenceModelWithEverythingSet
    {
        public PersistenceModelWithExtendedConfigurationAndEverythingSet(IPersistenceModel extendedModel)
        {
            ExtendConfigurationWith(extendedModel);
        }
    }
}
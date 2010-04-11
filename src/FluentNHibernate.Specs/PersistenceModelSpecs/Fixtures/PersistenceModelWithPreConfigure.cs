using System;
using NHibernate.Cfg;

namespace FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures
{
    class PersistenceModelWithPreConfigure : PersistenceModel
    {
        public PersistenceModelWithPreConfigure(Action<Configuration> preConfigureAction)
        {
            PreConfigure(preConfigureAction);
        }
    }
}
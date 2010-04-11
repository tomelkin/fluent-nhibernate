using System;
using NHibernate.Cfg;

namespace FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures
{
    class PersistenceModelWithPostConfigure : PersistenceModel
    {
        public PersistenceModelWithPostConfigure(Action<Configuration> postConfigureAction)
        {
            PostConfigure(postConfigureAction);
        }
    }
}
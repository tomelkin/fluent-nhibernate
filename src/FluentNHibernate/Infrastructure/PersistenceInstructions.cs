using System;
using System.Collections.Generic;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.Visitors;
using NHibernate.Cfg;

namespace FluentNHibernate.Infrastructure
{
    public interface IPersistenceInstructions
    {
        IEnumerable<IProviderSource> Sources { get; }
        ConventionsCollection Conventions { get; }
        IEnumerable<IMappingModelVisitor> Visitors { get; }
        IDatabaseConfiguration Database { get; }
        Action<Configuration> PreConfigure { get; }
        Action<Configuration> PostConfigure { get; }
    }

    public class PersistenceInstructions : IPersistenceInstructions
    {
        readonly List<IProviderSource> sources = new List<IProviderSource>();

        public PersistenceInstructions()
        {
            Conventions = new ConventionsCollection();
        }

        public IEnumerable<IProviderSource> Sources
        {
            get { return sources; }
        }

        public IDatabaseConfiguration Database { get; private set; }
        public Action<Configuration> PreConfigure { get; private set; }
        public Action<Configuration> PostConfigure { get; private set; }
        public ConventionsCollection Conventions { get; private set; }

        public IEnumerable<IMappingModelVisitor> Visitors
        {
            get
            {
                return new IMappingModelVisitor[]
                {
                    new SeparateSubclassVisitor(),
                    new ComponentReferenceResolutionVisitor(new IExternalComponentMappingProvider[0]),
                    new ComponentColumnPrefixVisitor(),
                    new BiDirectionalManyToManyPairingVisitor((a,b,c) => {}),
                    new ManyToManyTableNameVisitor(),
                    new ConventionVisitor(new ConventionFinder(Conventions)),
                    new ValidationVisitor()
                };
            }
        }

        public void AddSource(IProviderSource source)
        {
            sources.Add(source);
        }

        public void UseConventions(ConventionsCollection collection)
        {
            Conventions = collection;
        }

        public void UseDatabaseConfiguration(IDatabaseConfiguration dbCfg)
        {
            Database = dbCfg;
        }

        public void UsePreConfigureAction(Action<Configuration> preConfigureAction)
        {
            PreConfigure = preConfigureAction;
        }

        public void UsePostConfigureAction(Action<Configuration> postConfigureAction)
        {
            PostConfigure = postConfigureAction;
        }
    }
}
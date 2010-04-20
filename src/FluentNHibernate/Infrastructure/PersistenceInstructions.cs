using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.Visitors;
using NHibernate.Cfg;

namespace FluentNHibernate.Infrastructure
{
    public interface IPersistenceInstructions
    {
        IEnumerable<IMappingAction> GetActions();
        ConventionsCollection Conventions { get; }
        IEnumerable<IMappingModelVisitor> Visitors { get; }
        IDatabaseConfiguration Database { get; }
        Action<Configuration> PreConfigure { get; }
        Action<Configuration> PostConfigure { get; }
        IAutomappingInstructions AutomappingInstructions { get; }
    }

    public class PersistenceInstructions : IPersistenceInstructions
    {
        readonly List<IMappingAction> actions = new List<IMappingAction>();

        public PersistenceInstructions()
        {
            Conventions = new ConventionsCollection();
            AutomappingInstructions = new NullAutomappingInstructions();
        }

        public IDatabaseConfiguration Database { get; private set; }
        public Action<Configuration> PreConfigure { get; private set; }
        public Action<Configuration> PostConfigure { get; private set; }
        public ConventionsCollection Conventions { get; private set; }
        public IAutomappingInstructions AutomappingInstructions { get; private set; }

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

        public IEnumerable<IMappingAction> GetActions()
        {
            var partials = actions.Where(x => x is PartialAutomapAction);

            // combined automapping (do it all in one go)
            yield return AutomapAction.ComposeFrom(partials);

            // completely manual mappings
            foreach (var action in actions.Except(partials))
                yield return action;
        }

        public void AddActions(IEnumerable<IMappingAction> range)
        {
            actions.AddRange(range);
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

        public void UseAutomappingInstructions(IAutomappingInstructions instructions)
        {
            AutomappingInstructions = instructions;
        }
    }
}
using System;
using System.Collections.Generic;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.Visitors;
using NHibernate.Cfg;

namespace FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures
{
    class StubPersistenceModel : IPersistenceModel
    {
        readonly PersistenceInstructions instructions = new PersistenceInstructions();
        IEnumerable<IMappingAction> actions;

        public IAutomappingInstructions AutomappingInstructions
        {
            set { instructions.UseAutomappingInstructions(value); }
        }

        public ConventionsCollection Conventions
        {
            set { instructions.UseConventions(value); }
        }

        public IDatabaseConfiguration Database
        {
            set { instructions.UseDatabaseConfiguration(value); }
        }

        public IEnumerable<IMappingAction> Actions
        {
            set { actions = value; }
        }

        public Action<Configuration> PostConfigure
        {
            set { instructions.UsePostConfigureAction(value); }
        }

        public Action<Configuration> PreConfigure
        {
            set { instructions.UsePreConfigureAction(value); }
        }

        public IPersistenceInstructions GetInstructions()
        {
            instructions.AddActions(actions);
            return instructions;
        }
    }
}
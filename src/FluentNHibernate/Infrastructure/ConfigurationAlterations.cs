using System.Collections.Generic;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.MappingModel;
using NHibernate.Cfg;

namespace FluentNHibernate.Infrastructure
{
    public class ConfigurationAlterations
    {
        readonly IPersistenceInstructions instructions;

        public IEnumerable<HibernateMapping> Mappings { get; private set; }
        
        public IDatabaseConfiguration Database
        {
            get { return instructions.Database; }
        }

        public ConfigurationAlterations(IEnumerable<HibernateMapping> mappings, IPersistenceInstructions instructions)
        {
            this.instructions = instructions;
            Mappings = mappings;
        }

        public void PreConfigure(Configuration cfg)
        {
            if (instructions.PreConfigure != null)
                instructions.PreConfigure(cfg);
        }

        public void PostConfigure(Configuration cfg)
        {
            if (instructions.PostConfigure != null)
                instructions.PostConfigure(cfg);
        }
    }
}
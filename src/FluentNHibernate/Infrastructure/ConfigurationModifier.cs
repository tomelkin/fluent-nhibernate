using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Output;
using NHibernate.Cfg;

namespace FluentNHibernate.Infrastructure
{
    public class ConfigurationModifier
    {
        readonly ConfigurationAlterations alterations;

        public ConfigurationModifier(ConfigurationAlterations alterations)
        {
            this.alterations = alterations;
        }

        public void Inject(Configuration cfg)
        {
            alterations.PreConfigure(cfg);

            UpdateSettings(cfg, alterations.Database);
            InjectMappings(cfg, alterations.Mappings);

            alterations.PostConfigure(cfg);
        }

        void UpdateSettings(Configuration cfg, IDatabaseConfiguration database)
        {
            if (database != null)
                database.Configure(cfg);
        }

        void InjectMappings(Configuration cfg, IEnumerable<HibernateMapping> mappings)
        {
            foreach (var mapping in mappings.Where(m => m.Classes.Count() == 0))
            {
                var serializer = new MappingXmlSerializer();
                var document = serializer.Serialize(mapping);
                cfg.AddDocument(document);
            }

            foreach (var mapping in mappings.Where(m => m.Classes.Count() > 0))
            {
                var serializer = new MappingXmlSerializer();
                var document = serializer.Serialize(mapping);

                if (cfg.GetClassMapping(mapping.Classes.First().Type) == null)
                {
                    try
                    {
                        cfg.AddDocument(document);
                    }
                    catch (Exception ex)
                    {
                        throw new FluentConfigurationException("Error adding mapping.", ex);
                    }
                }
            }
        }
    }
}
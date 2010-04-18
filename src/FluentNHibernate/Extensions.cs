using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using FluentNHibernate.Automapping;
using NHibernate.Cfg;
using NHibernate.Util;

namespace FluentNHibernate
{
    public static class ConfigurationHelper
    {
        public static Configuration ConfigureWith(this Configuration cfg, PersistenceModel model)
        {
            return cfg.ConfigureWith((IPersistenceInstructionGatherer)model);
        }

        public static Configuration ConfigureWith(this Configuration cfg, IPersistenceInstructionGatherer gatherer)
        {
            // TODO: move this out of an extension method
            var instructions = gatherer.GetInstructions();
            var compiler = new MappingCompiler(instructions);
            var mappings = compiler.BuildMappings();
            var injector = new MappingInjector(mappings);

            injector.Inject(cfg);

            return cfg;
        }

        public static Configuration AddMappingsFromAssembly(this Configuration configuration, Assembly assembly)
        {
            var models = new PersistenceModel();
            models.AddMappingsFromAssembly(assembly);
            //models.Configure(configuration);

            return configuration;
        }

        public static Configuration AddAutoMappings(this Configuration configuration, AutoPersistenceModel model)
        {
            model.Configure(configuration);

            return configuration;
        }
    }
}
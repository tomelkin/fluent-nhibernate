using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Infrastructure;
using NHibernate.Cfg;

namespace FluentNHibernate
{
    public static class ConfigurationHelper
    {
        public static Configuration ConfigureWith<T>(this Configuration cfg)
            where T : PersistenceModel, new()
        {
            return cfg.ConfigureWith(new T());
        }

        public static Configuration ConfigureWith(this Configuration cfg, PersistenceModel model)
        {
            return cfg.ConfigureWith((IPersistenceInstructionGatherer)model);
        }

        public static Configuration ConfigureWith(this Configuration cfg, IPersistenceInstructionGatherer gatherer)
        {
            // TODO: move this out of an extension method
            var instructions = gatherer.GetInstructions();
            var automapper = new AutomapperV2(new ConventionFinder(instructions.Conventions));
            var compiler = new MappingCompiler(automapper, instructions);
            var mappings = compiler.BuildMappings();
            var alterations = new ConfigurationAlterations(mappings, instructions);
            var injector = new ConfigurationModifier(alterations);

            injector.Inject(cfg);

            return cfg;
        }
    }
}
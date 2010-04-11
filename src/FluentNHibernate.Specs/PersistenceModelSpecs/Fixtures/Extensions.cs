using FluentNHibernate.Infrastructure;
using FluentNHibernate.MappingModel;
using NHibernate.Cfg;

namespace FluentNHibernate.Specs.PersistenceModelSpecs.Fixtures
{
    static class Extensions
    {
        /// <summary>
        /// Test Extension - Gets the <see cref="IPersistenceInstructions"/> from a
        /// <see cref="IPersistenceInstructionGatherer"/> instance.
        /// </summary>
        /// <param name="model"><see cref="IPersistenceInstructionGatherer"/> instance</param>
        /// <returns>Persistence instructions from the model</returns>
        public static IPersistenceInstructions GetInstructions(this IPersistenceInstructionGatherer model)
        {
            return model.GetInstructions();
        }

        /// <summary>
        /// Test Extension - Applies a set of instructions to an NHibernate configuration instance.
        /// </summary>
        /// <param name="instructions">Instructions to apply</param>
        /// <param name="cfg">Target configuration instance</param>
        public static void Apply(this IPersistenceInstructions instructions, Configuration cfg)
        {
            var alterations = new ConfigurationAlterations(new HibernateMapping[0], instructions);

            new ConfigurationModifier(alterations)
                .Inject(cfg);
        }
    }
}

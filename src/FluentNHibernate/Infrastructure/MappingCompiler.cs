using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Infrastructure
{
    public interface IMappingCompiler
    {
        IEnumerable<HibernateMapping> BuildMappings();
    }

    public class MappingCompiler : IMappingCompiler
    {
        readonly IPersistenceInstructions instructions;

        public MappingCompiler(IPersistenceInstructions instructions)
        {
            this.instructions = instructions;
        }

        public IEnumerable<HibernateMapping> BuildMappings()
        {
            var mappings = CompileProviders(instructions.Sources);

            instructions.Visitors
                .Each(x => x.Visit(mappings));

            return mappings;
        }

        IEnumerable<HibernateMapping> CompileProviders(IEnumerable<IProviderSource> sources)
        {
            var topMappings = sources
                .SelectMany(x => x.Compile(this).CompiledMappings);
            var hbm = new HibernateMapping();

            topMappings.Each(x => x.AddTo(hbm));

            return new[] { hbm };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Infrastructure
{
    public interface IMappingCompiler
    {
        ITopMapping AutoMap(ITopMapping mapping);
        ITopMapping ManualMap(ITopMapping mapping);
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

        public virtual ITopMapping AutoMap(ITopMapping mapping)
        {
            var automapping = instructions.AutomappingInstructions;

            return mapping;
        }

        public virtual ITopMapping ManualMap(ITopMapping mapping)
        {
            return mapping;
        }
    }
}
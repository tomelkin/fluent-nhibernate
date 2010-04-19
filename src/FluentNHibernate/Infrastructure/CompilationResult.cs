using System.Collections.Generic;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Infrastructure
{
    public class CompilationResult
    {
        public CompilationResult(IEnumerable<ITopMapping> compiledMappings)
        {
            CompiledMappings = compiledMappings;
        }

        public IEnumerable<ITopMapping> CompiledMappings { get; private set; }
    }
}
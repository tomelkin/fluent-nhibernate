using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Infrastructure
{
    public class FluentMappingSource : IProviderSource
    {
        readonly IEnumerable<IProvider> instances;

        public FluentMappingSource(IEnumerable<IProvider> instances)
        {
            this.instances = instances;
        }

        public CompilationResult Compile(IMappingCompiler mappingCompiler)
        {
            var compiledMappings = new List<ITopMapping>();

            instances
                .Select(x => x.GetAction())
                .Select(x => x.Execute(mappingCompiler))
                .Each(compiledMappings.Add);

            return new CompilationResult(compiledMappings);
        }
    }
}
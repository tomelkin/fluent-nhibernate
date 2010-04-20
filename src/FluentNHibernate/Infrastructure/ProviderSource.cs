using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Infrastructure
{
    public class ProviderSource : IProviderSource
    {
        readonly IEnumerable<IProvider> instances;

        public ProviderSource(IEnumerable<IProvider> instances)
        {
            this.instances = instances;
        }

        public CompilationResult Compile(IMappingCompiler mappingCompiler)
        {
            var compiledMappings = new List<ITopMapping>();
            
            instances
                .Select(x => x.GetAction())
                .SelectMany(x => mappingCompiler.Compile(x))
                .Each(compiledMappings.Add);

            return new CompilationResult(compiledMappings);
        }
    }
}
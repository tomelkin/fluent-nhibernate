using System;
using System.Collections.Generic;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate
{
    /// <summary>
    /// A source for Type instances, acts as a facade for an Assembly or as an alternative Type provider.
    /// </summary>
    public interface ITypeSource
    {
        IEnumerable<Type> GetTypes();
    }

    public interface IProviderSource
    {
        CompilationResult Compile(IMappingCompiler mappingCompiler);
    }

    public class CompilationResult
    {
        public CompilationResult(IEnumerable<ITopMapping> compiledMappings)
        {
            CompiledMappings = compiledMappings;
        }

        public IEnumerable<ITopMapping> CompiledMappings { get; private set; }
    }
}
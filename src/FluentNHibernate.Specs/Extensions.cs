using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using Machine.Specifications;

namespace FluentNHibernate.Specs
{
    public static class Extensions
    {
        public static T As<T>(this object instance)
        {
            return (T)instance;
        }

        public static void ShouldContain<T>(this IEnumerable<T> collection, Func<T, bool> predicate)
        {
            collection.Any(predicate).ShouldBeTrue();
        }

        public static IEnumerable<HibernateMapping> BuildMappings(this IPersistenceInstructions instructions)
        {
            return new MappingCompiler(instructions)
                .BuildMappings();
        }

        public static ClassMapping GetClassMapping(this IProvider provider)
        {
            var instructions = new PersistenceInstructions();

            instructions.AddSource(new StubProviderSource(provider));

            var compiler = new MappingCompiler(instructions);

            return compiler.BuildMappings()
                .SelectMany(x => x.Classes)
                .First();
        }
    }

    public class StubProviderSource : IProviderSource
    {
        readonly IEnumerable<IProvider> providers;

        public StubProviderSource(params IProvider[] providers)
        {
            this.providers = providers;
        }

        public StubProviderSource(IEnumerable<IProvider> providers)
        {
            this.providers = providers;
        }

        public CompilationResult Compile(IMappingCompiler mappingCompiler)
        {
            return new CompilationResult(providers.Select(x => x.GetMapping()));
        }
    }
}

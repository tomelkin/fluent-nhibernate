using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.Infrastructure;
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
            return new MappingCompiler(new AutomapperV2(new ConventionFinder(instructions.Conventions)), instructions)
                .BuildMappings();
        }

        public static ClassMapping GetClassMapping(this IProvider provider)
        {
            var instructions = new PersistenceInstructions();

            instructions.AddActions(provider.GetAction());

            return instructions.BuildMappings()
                .SelectMany(x => x.Classes)
                .First();
        }

        public static ClassMapping BuildMappingFor<T>(this MappingCompiler compiler)
        {
            return compiler.BuildMappings()
                .SelectMany(x => x.Classes)
                .FirstOrDefault(x => x.Type == typeof(T));
        }

        public static void AddActions(this PersistenceInstructions instructions, params IProvider[] providers)
        {
            instructions.AddActions(providers.Select(x => x.GetAction()));
        }

        public static void AddActions(this PersistenceInstructions instructions, params IMappingAction[] actions)
        {
            instructions.AddActions(actions);
        }

        public static ClassMapping MapEntity<T>(this AutomapperV2 mapper)
        {
            var mappings = mapper.Map(new[]
            {
                new AutomappingTarget(typeof(T), new ClassMapping { Type = typeof(T) }, new AutomappingInstructions())
            });

            return (ClassMapping)mappings.FirstOrDefault(x => x.Type == typeof(T));
        }

        class StubProvider : IProvider
        {
            readonly IMappingAction action;

            public StubProvider(IMappingAction action)
            {
                this.action = action;
            }

            public IMappingAction GetAction()
            {
                return action;
            }
        }
    }

    public static class Mapping
    {
        public static ClassMapping For<T>()
        {
            return new ClassMapping { Type = typeof(T) };
        }
    }

    public static class Action
    {
        public static PartialAutomapAction For<T>()
        {
            return new PartialAutomapAction(Mapping.For<T>());
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
            var actions = providers.Select(x => x.GetAction());
            var mappings = actions.SelectMany(x => mappingCompiler.Compile(x));

            return new CompilationResult(mappings);
        }
    }
}

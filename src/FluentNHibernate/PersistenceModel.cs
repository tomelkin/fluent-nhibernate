using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Output;
using FluentNHibernate.Utils;
using FluentNHibernate.Visitors;
using NHibernate.Cfg;

namespace FluentNHibernate
{
    public interface IPersistenceInstructions
    {
        IEnumerable<IProviderSource> Sources { get; }
        ConventionsCollection Conventions { get; }
        IEnumerable<IMappingModelVisitor> Visitors { get; }
    }

    public interface IPersistenceInstructionGatherer
    {
        IPersistenceInstructions GetInstructions();
    }

    public class MappingInjector
    {
        readonly IEnumerable<HibernateMapping> mappings;

        public MappingInjector(IEnumerable<HibernateMapping> mappings)
        {
            this.mappings = mappings;
        }

        public void Inject(Configuration cfg)
        {
            foreach (var mapping in mappings.Where(m => m.Classes.Count() == 0))
            {
                var serializer = new MappingXmlSerializer();
                var document = serializer.Serialize(mapping);
                cfg.AddDocument(document);
            }

            foreach (var mapping in mappings.Where(m => m.Classes.Count() > 0))
            {
                var serializer = new MappingXmlSerializer();
                var document = serializer.Serialize(mapping);

                if (cfg.GetClassMapping(mapping.Classes.First().Type) == null)
                    cfg.AddDocument(document);
            }
        }
    }

    public interface IMappingCompiler
    {
        
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

            instructions
                .Visitors
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

    public class PersistenceModel : IPersistenceInstructionGatherer
    {
        readonly List<ITypeSource> sources = new List<ITypeSource>();
        readonly List<IProvider> instances = new List<IProvider>();
        readonly ConventionsCollection conventions = new ConventionsCollection();

        public IConventionContainer Conventions
        {
            get { return new ConventionContainer(conventions); }
        }

        protected void AddMappingsFromThisAssembly()
        {
            var assembly = FindTheCallingAssembly();
            AddMappingsFromAssembly(assembly);
        }

        public void AddMappingsFromAssembly(Assembly assembly)
        {
            AddMappingsFromSource(new AssemblyTypeSource(assembly));
        }

        public void Add(IProvider provider)
        {
            instances.Add(provider);
        }

        public void AddMappingsFromSource(ITypeSource source)
        {
            sources.Add(source);
        }

        public void AddMappings(params IProvider[] providers)
        {
            instances.AddRange(providers);
        }

        private static Assembly FindTheCallingAssembly()
        {
            StackTrace trace = new StackTrace(Thread.CurrentThread, false);

            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            Assembly callingAssembly = null;
            for (int i = 0; i < trace.FrameCount; i++)
            {
                StackFrame frame = trace.GetFrame(i);
                Assembly assembly = frame.GetMethod().DeclaringType.Assembly;
                if (assembly != thisAssembly)
                {
                    callingAssembly = assembly;
                    break;
                }
            }
            return callingAssembly;
        }

        IPersistenceInstructions IPersistenceInstructionGatherer.GetInstructions()
        {
            var instructions = new PersistenceInstructions();

            var mappingProviders = new List<IProvider>();

            mappingProviders.AddRange(instances); // add pre-instantiated maps
            mappingProviders.AddRange(GetProvidersFromSources());

            instructions.AddSource(new FluentMappingSource(mappingProviders));
            instructions.UseConventions(conventions);

            return instructions;
        }

        IEnumerable<IProvider> GetProvidersFromSources()
        {
            // TODO: Add user-defined filtering in here
            return sources
                .SelectMany(x => x.GetTypes())
                .Where(x => x.HasInterface<IProvider>())
                .Select(x => x.InstantiateUsingParameterlessConstructor())
                .Cast<IProvider>();
        }

        class SingleTypeSource : ITypeSource
        {
            readonly Type type;

            public SingleTypeSource(Type type)
            {
                this.type = type;
            }

            public IEnumerable<Type> GetTypes()
            {
                return new[] {type};
            }
        }
    }

    public class PersistenceInstructions : IPersistenceInstructions
    {
        readonly List<IProviderSource> sources = new List<IProviderSource>();

        public PersistenceInstructions()
        {
            Conventions = new ConventionsCollection();
        }

        public IEnumerable<IProviderSource> Sources
        {
            get { return sources; }
        }

        public ConventionsCollection Conventions { get; private set; }
        
        public IEnumerable<IMappingModelVisitor> Visitors
        {
            get
            {
                return new IMappingModelVisitor[]
                {
                    new SeparateSubclassVisitor(new List<IIndeterminateSubclassMappingProvider>()),
                    new ComponentReferenceResolutionVisitor(new IExternalComponentMappingProvider[0]),
                    new ComponentColumnPrefixVisitor(),
                    new BiDirectionalManyToManyPairingVisitor((a,b,c) => {}),
                    new ManyToManyTableNameVisitor(),
                    new ConventionVisitor(new ConventionFinder(Conventions)),
                    new ValidationVisitor()
                };
            }
        }

        public void AddSource(IProviderSource source)
        {
            sources.Add(source);
        }

        public void UseConventions(ConventionsCollection collection)
        {
            Conventions = collection;
        }
    }

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
                .Select(x => x.GetMapping())
                .Each(compiledMappings.Add);

            return new CompilationResult(compiledMappings);
        }
    }

    public interface IProvider
    {
        ITopMapping GetMapping();
    }

    public class PassThroughMappingProvider : IProvider
    {
        private readonly ClassMapping mapping;

        public PassThroughMappingProvider(ClassMapping mapping)
        {
            this.mapping = mapping;
        }

        public ITopMapping GetMapping()
        {
            return mapping;
        }
    }
}
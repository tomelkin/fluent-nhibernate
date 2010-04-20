using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Infrastructure;

namespace FluentNHibernate
{
    public interface IAutomappingInstructions
    {
        IAutomappingConfiguration Configuration { get; }
        void UseConfiguration(IAutomappingConfiguration cfg);
        void AddSource(ITypeSource source);
        IEnumerable<Type> GetTypesToMap();
    }

    public class AutomappingInstructions : IAutomappingInstructions
    {
        readonly List<ITypeSource> sources = new List<ITypeSource>();
        public IAutomappingConfiguration Configuration { get; private set; }

        public AutomappingInstructions()
        {
            Configuration = new DefaultAutomappingConfiguration();
        }

        public void UseConfiguration(IAutomappingConfiguration cfg)
        {
            Configuration = cfg;
        }

        public void AddSource(ITypeSource source)
        {
            sources.Add(source);
        }

        public IEnumerable<Type> GetTypesToMap()
        {
            return sources
                .SelectMany(x => x.GetTypes())
                .Where(x => Configuration.ShouldMap(x))
                .ToArray();
        }
    }

    public class NullAutomappingInstructions : IAutomappingInstructions
    {
        public IAutomappingConfiguration Configuration
        {
            get { return null; }
        }

        public void UseConfiguration(IAutomappingConfiguration cfg)
        {
            throw new NotSupportedException("Cannot set configuration in null instructions");
        }

        public void AddSource(ITypeSource source)
        {
            throw new NotSupportedException("Cannot add source in null instructions");
        }

        public IEnumerable<Type> GetTypesToMap()
        {
            return new Type[0];
        }
    }
}
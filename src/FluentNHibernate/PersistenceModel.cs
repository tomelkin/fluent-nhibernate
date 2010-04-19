using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.Utils;
using NHibernate.Cfg;

namespace FluentNHibernate
{
    public interface IPersistenceModel
    {}

    public class PersistenceModel : IPersistenceInstructionGatherer
    {
        readonly List<ITypeSource> sources = new List<ITypeSource>();
        readonly List<IProvider> instances = new List<IProvider>();
        readonly ConventionsCollection conventions = new ConventionsCollection();
        IDatabaseConfiguration databaseConfiguration;
        Action<Configuration> preConfigure;
        Action<Configuration> postConfigure;

        protected void PreConfigure(Action<Configuration> preConfigureAction)
        {
            preConfigure = preConfigureAction;
        }

        protected void PostConfigure(Action<Configuration> postConfigureAction)
        {
            postConfigure = postConfigureAction;
        }

        /// <summary>
        /// Base the PersistenceModel configuration on another PersistenceModel's setup.
        /// Use this method to "inherit" settings, that can be overwritten in your own
        /// model.
        /// </summary>
        /// <param name="model">PersistenceModel to inherit settings from</param>
        protected void BaseConfigurationOn(IPersistenceModel model)
        {
            
        }

        /// <summary>
        /// Extend the PersistenceModel configuration with another PersistenceModel's setup.
        /// Use this method to apply existing settings "on top" of your own settings. Good
        /// for if you want to pass in a "test" configuration that just alters minor settings but
        /// keeps everything else intact.
        /// </summary>
        /// <param name="model">PersistenceModel to extend your own with</param>
        protected void ExtendConfigurationWith(IPersistenceModel model)
        {
            
        }

        protected void Database(IPersistenceConfigurer db)
        {
            databaseConfiguration = new PreconfiguredDatabaseConfiguration(db);
        }

        protected void Database(IDatabaseConfiguration dbCfg)
        {
            databaseConfiguration = dbCfg;
        }

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
            var trace = new StackTrace(Thread.CurrentThread, false);

            var thisAssembly = Assembly.GetExecutingAssembly();
            Assembly callingAssembly = null;
            for (var i = 0; i < trace.FrameCount; i++)
            {
                var frame = trace.GetFrame(i);
                var assembly = frame.GetMethod().DeclaringType.Assembly;
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

            if (databaseConfiguration != null)
                instructions.UseDatabaseConfiguration(databaseConfiguration);

            if (preConfigure != null)
                instructions.UsePreConfigureAction(preConfigure);

            if (postConfigure != null)
                instructions.UsePostConfigureAction(postConfigure);

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
    }
}
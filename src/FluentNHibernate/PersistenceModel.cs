using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;
using FluentNHibernate.Utils.Reflection;
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
        IAutomappingInstructions automapping;

        public AutomappingBuilder AutoMap
        {
            get { return new AutomappingBuilder(automapping ?? (automapping = new AutomappingInstructions())); }
        }

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
            var assembly = ReflectionHelper.FindTheCallingAssembly();
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

        IEnumerable<IMappingAction> GetActions()
        {
            var actionsFromInstances = instances.Select(x => x.GetAction());
            var actionsFromProviders = GetProvidersFromSources().Select(x => x.GetAction());

            // all pre-instantiated providers)
            foreach (var action in actionsFromInstances)
                yield return action;

            // all providers found by scanning
            foreach (var action in actionsFromProviders)
                yield return action;

            // all types for mapping by the automapper
            if (automapping != null)
            {
                var actionsForAutomapping = automapping.GetTypesToMap().Select(x => new PartialAutomapAction(x, new AutomappingEntitySetup()));
                
                foreach (var action in actionsForAutomapping)
                    yield return action;
            }
        }

        IPersistenceInstructions IPersistenceInstructionGatherer.GetInstructions()
        {
            var instructions = new PersistenceInstructions();
            var actions = GetActions();

            instructions.AddActions(actions);
            instructions.UseConventions(conventions);

            if (databaseConfiguration != null)
                instructions.UseDatabaseConfiguration(databaseConfiguration);

            if (preConfigure != null)
                instructions.UsePreConfigureAction(preConfigure);

            if (postConfigure != null)
                instructions.UsePostConfigureAction(postConfigure);

            if (automapping != null)
                instructions.UseAutomappingInstructions(automapping);

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
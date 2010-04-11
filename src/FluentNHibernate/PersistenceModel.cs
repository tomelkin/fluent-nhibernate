using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;
using FluentNHibernate.Utils.Reflection;
using NHibernate.Cfg;

namespace FluentNHibernate
{
    public interface IPersistenceModel : IPersistenceInstructionGatherer
    {}

    public class PersistenceModel : IPersistenceModel
    {
        readonly List<ITypeSource> sources = new List<ITypeSource>();
        readonly List<IProvider> instances = new List<IProvider>();
        readonly ConventionsCollection conventions = new ConventionsCollection();
        IDatabaseConfiguration databaseConfiguration;
        Action<Configuration> preConfigure;
        Action<Configuration> postConfigure;
        IAutomappingInstructions automapping;
        IPersistenceInstructionGatherer baseModel;
        IPersistenceInstructionGatherer extendedModel;

        public AutomappingBuilder AutoMap
        {
            get { return new AutomappingBuilder(automapping ?? (automapping = new AutomappingInstructions())); }
        }

        /// <summary>
        /// Specify an action that is executed before any changes are made by Fluent NHibernate to
        /// the NHibernate <see cref="Configuration"/> instance. This method can only be called once,
        /// multiple calls will result in only the last call being used.
        /// </summary>
        /// <example>
        /// 1:
        /// <code>
        /// PreConfigure(cfg =>
        /// {
        /// });
        /// </code>
        /// 
        /// 2:
        /// <code>
        /// PreConfigure(someMethod);
        /// 
        /// private void someMethod(Configuration cfg)
        /// {
        /// }
        /// </code>
        /// </example>
        /// <param name="preConfigureAction">Action to execute</param>
        protected void PreConfigure(Action<Configuration> preConfigureAction)
        {
            preConfigure = preConfigureAction;
        }

        /// <summary>
        /// Specify an action that is executed after all other changes have been made by Fluent
        /// NHibernate to the NHibernate <see cref="Configuration"/> instance. This method can
        /// only be called once, multiple calls will result in only the last call being used.
        /// </summary>
        /// <example>
        /// See <see cref="PreConfigure"/> for examples.
        /// </example>
        /// <param name="postConfigureAction">Action to execute</param>
        protected void PostConfigure(Action<Configuration> postConfigureAction)
        {
            postConfigure = postConfigureAction;
        }

        /// <summary>
        /// Base this configuration on another <see cref="IPersistenceModel"/>'s setup.
        /// Use this method to "inherit" settings, that can be overwritten in your own
        /// model. This method can only be called once, multiple calls will result in only the last call
        /// being used.
        /// </summary>
        /// <param name="model">PersistenceModel to inherit settings from</param>
        protected void BaseConfigurationOn(IPersistenceModel model)
        {
            BaseConfigurationOn((IPersistenceInstructionGatherer)model);
        }

        /// <summary>
        /// Base this configuration on another <see cref="IPersistenceModel"/>'s setup.
        /// Use this method to "inherit" settings, that can be overwritten in your own
        /// model. This method can only be called once, multiple calls will result in only the last call
        /// being used.
        /// </summary>
        /// <param name="instrucionGather">PersistenceModel to inherit settings from</param>
        protected void BaseConfigurationOn(IPersistenceInstructionGatherer instrucionGather)
        {
            baseModel = instrucionGather;
        }

        /// <summary>
        /// Extend ththis configuration with another PersistenceModel's setup.
        /// Use this method to apply existing settings "on top" of your own settings. Good
        /// for if you want to pass in a "test" configuration that just alters minor settings but
        /// keeps everything else intact. This method can only be called once, multiple calls will
        /// result in only the last call being used.
        /// </summary>
        /// <param name="model">PersistenceModel to extend your own with</param>
        protected void ExtendConfigurationWith(IPersistenceModel model)
        {
            ExtendConfigurationWith((IPersistenceInstructionGatherer)model);
        }

        /// <summary>
        /// Extend ththis configuration with another PersistenceModel's setup.
        /// Use this method to apply existing settings "on top" of your own settings. Good
        /// for if you want to pass in a "test" configuration that just alters minor settings but
        /// keeps everything else intact. This method can only be called once, multiple calls will
        /// result in only the last call being used.
        /// </summary>
        /// <param name="instructionGatherer">PersistenceModel to extend your own with</param>
        protected void ExtendConfigurationWith(IPersistenceInstructionGatherer instructionGatherer)
        {
            extendedModel = instructionGatherer;
        }

        /// <summary>
        /// Supply settings for the database used in the persistence of your entities.
        /// This method can only be called once, multiple calls will result in only
        /// the last call being used.
        /// </summary>
        /// <remarks>
        /// Where the instance comes from that you pass into this method
        /// is up to you. You can instantiate it yourself, or you could
        /// inject it into your <see cref="PersistenceModel"/> via a
        /// container and pass it into this method.
        /// </remarks>
        /// <example>
        /// Inline:
        /// <code>
        /// Database(new ProductionDatabaseConfiguration());
        /// </code>
        /// 
        /// Container:
        /// <code>
        /// public class MyPersistenceModel : PersistenceModel
        /// {
        ///   public MyPersistenceModel(IDatabaseConfiguration dbCfg)
        ///   {
        ///     Database(dbCfg);
        ///   }
        /// }
        /// </code>
        /// </example>
        /// <param name="dbCfg">Database configuration instance</param>
        protected void Database(IDatabaseConfiguration dbCfg)
        {
            databaseConfiguration = dbCfg;
        }

        /// <summary>
        /// Supply settings for the database used in the persistence of your entities.
        /// This method can only be called once, multiple calls will result in only
        /// the last call being used.
        /// </summary>
        /// <example>
        /// See <see cref="Database(FluentNHibernate.Cfg.Db.IDatabaseConfiguration)"/> for examples.
        /// </example>
        /// <typeparam name="T">Type of database configuration</typeparam>
        protected void Database<T>()
            where T : IDatabaseConfiguration, new()
        {
            Database(new T());
        }

        /// <summary>
        /// Supply settings, in the form of an inline setup, for the database used in the persistence
        /// of your entities. This method can only be called once, multiple calls will result in only
        /// the last call being used.
        /// </summary>
        /// <remarks>
        /// The parameter to this method will be wrapped inside a <see cref="IDatabaseConfiguration"/>
        /// instance. This method is mainly useful for short inline database configuration.
        /// </remarks>
        /// <example>
        /// <code>
        /// Database(SQLiteConfiguration.StandardInMemory);
        /// </code>
        /// </example>
        /// <param name="db">Persistence configurer instance</param>
        protected void Database(IPersistenceConfigurer db)
        {
            databaseConfiguration = new PreconfiguredDatabaseConfiguration(db);
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

        IPersistenceInstructions CreateInstructions()
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

        IPersistenceInstructions IPersistenceInstructionGatherer.GetInstructions()
        {
            var instructions = CreateInstructions();
            
            if (baseModel != null)
                instructions = new DerivedPersistenceInstructions(baseModel.GetInstructions(), instructions);

            if (extendedModel != null)
                instructions = new ExtendedPersistenceInstructions(extendedModel.GetInstructions(), instructions);

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
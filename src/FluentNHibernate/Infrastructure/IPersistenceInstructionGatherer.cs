using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;
using NHibernate.Cfg;

namespace FluentNHibernate.Infrastructure
{
    public interface IPersistenceInstructionGatherer
    {
        IPersistenceInstructions GetInstructions();
    }

    public class PersistenceInstructionGatherer : IPersistenceInstructionGatherer
    {
        readonly List<ITypeSource> sources = new List<ITypeSource>();
        readonly List<IProvider> instances = new List<IProvider>();
        readonly ConventionsCollection conventions = new ConventionsCollection();
        readonly IAutomappingInstructions automapping = new AutomappingInstructions();
        IPersistenceInstructionGatherer baseModel;
        IPersistenceInstructionGatherer extendedModel;
        Action<Configuration> preConfigure;
        Action<Configuration> postConfigure;
        IDatabaseConfiguration databaseConfiguration;

        public IPersistenceInstructions GetInstructions()
        {
            var instructions = CreateInstructions();

            if (baseModel != null)
                instructions = new DerivedPersistenceInstructions(baseModel.GetInstructions(), instructions);

            if (extendedModel != null)
                instructions = new ExtendedPersistenceInstructions(extendedModel.GetInstructions(), instructions);

            return instructions;
        }

        public ConventionsCollection Conventions
        {
            get { return conventions; }
        }

        public IAutomappingInstructions Automapping
        {
            get { return automapping; }
        }

        public void AddProviderInstance(IProvider provider)
        {
            instances.Add(provider);
        }

        public void AddSource(ITypeSource source)
        {
            sources.Add(source);
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

        public void UseBaseModel(IPersistenceInstructionGatherer instrucionGather)
        {
            baseModel = instrucionGather;
        }

        public void UseExtendModel(IPersistenceInstructionGatherer instructionGatherer)
        {
            extendedModel = instructionGatherer;
        }

        public void UsePreConfigure(Action<Configuration> preConfigureAction)
        {
            preConfigure = preConfigureAction;
        }

        public void UsePostConfigure(Action<Configuration> postConfigureAction)
        {
            postConfigure = postConfigureAction;
        }

        public void UseDatabaseConfiguration(IDatabaseConfiguration dbCfg)
        {
            databaseConfiguration = dbCfg;
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
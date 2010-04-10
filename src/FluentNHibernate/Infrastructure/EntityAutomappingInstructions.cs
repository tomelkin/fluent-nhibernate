using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Infrastructure
{
    // TODO: Use this class to combine the main instructions with individual
    // entity instructions. Currently just delegates everything to the main instructions
    public class EntityAutomappingInstructions : IEntityAutomappingInstructions
    {
        readonly IAutomappingInstructions mainInstructions;
        readonly AutomappingEntitySetup setup;

        public EntityAutomappingInstructions(IAutomappingInstructions mainInstructions, AutomappingEntitySetup setup)
        {
            this.mainInstructions = mainInstructions;
            this.setup = setup;
        }

        public IEntityAutomappingConfiguration Configuration
        {
            get { return GetConfiguration(); }
        }

        IEntityAutomappingConfiguration GetConfiguration()
        {
            var innerCfg = setup.Configuration ?? mainInstructions.Configuration ?? new DefaultAutomappingConfiguration();

            if (!setup.Exclusions.Any())
                return innerCfg;

            return new ExclusionWrappedConfiguration(innerCfg, setup.Exclusions);
        }

        public class ExclusionWrappedConfiguration : IEntityAutomappingConfiguration
        {
            readonly IAutomappingConfiguration innerCfg;
            readonly IEnumerable<Predicate<Member>> exclusions;

            public ExclusionWrappedConfiguration(IAutomappingConfiguration innerCfg, IEnumerable<Predicate<Member>> exclusions)
            {
                this.innerCfg = innerCfg;
                this.exclusions = exclusions;
            }

            public bool ShouldMap(Member member)
            {
                return innerCfg.ShouldMap(member) && !exclusions.Any(x => x(member));
            }

            public IEnumerable<IAutomappingStep> GetMappingSteps(IAutomapper mapper, IConventionFinder conventionFinder)
            {
                return innerCfg.GetMappingSteps(mapper, conventionFinder);
            }
        }
    }
}
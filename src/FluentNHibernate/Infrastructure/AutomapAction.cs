using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentNHibernate.Infrastructure
{
    public class AutomapAction : IMappingAction
    {
        readonly IEnumerable<PartialAutomapAction> actions;

        public AutomapAction(IEnumerable<PartialAutomapAction> actions)
        {
            this.actions = actions;
        }

        public static AutomapAction ComposeFrom(IEnumerable<IMappingAction> partials)
        {
            if (partials.Any(x => !(x is PartialAutomapAction)))
                throw new ArgumentException("partials must all be of type PartialAutomapAction");

            return new AutomapAction(partials.Cast<PartialAutomapAction>());
        }

        public IEnumerable<AutomappingTarget> GetMappingTargets(IAutomappingInstructions mainInstructions)
        {
            return actions.Select(x => x.CreateTarget(mainInstructions));
        }
    }
}
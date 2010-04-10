using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Conventions;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping
{
    public interface IAutomapper
    {
        IEnumerable<ITopMapping> Map(IEnumerable<AutomappingTarget> targets);
    }

    public class AutomapperV2 : IAutomapper
    {
        readonly IConventionFinder conventions;

        public AutomapperV2(IConventionFinder conventions)
        {
            this.conventions = conventions;
        }

        public IEnumerable<ITopMapping> Map(IEnumerable<AutomappingTarget> targets)
        {
            return targets
                .Select(x => Map(x))
                .ToArray();
        }

        ITopMapping Map(AutomappingTarget target)
        {
            var instructions = target.Instructions;
            var config = instructions.Configuration;
            var mapping = (ClassMapping)target.Mapping; // TODO: Find a way around this cast
            var steps = config.GetMappingSteps(this, conventions);

            // TODO: Change this to use defaults or something
            mapping.Name = target.Type.AssemblyQualifiedName;

            mapping.Type.GetInstanceMembers()
                .Where(x => config.ShouldMap(x) && !target.IsMemberUsed(x))
                .Select(x => MapMember(target, x, steps))
                .Where(x => x != null)
                .Each(mapping.AddMappedMember);

            return mapping;
        }

        IMemberMapping MapMember(AutomappingTarget target, Member member, IEnumerable<IAutomappingStep> steps)
        {
            var appropriateStep = steps.FirstOrDefault(x => x.ShouldMap(target, member));

            if (appropriateStep != null)
                return appropriateStep.Map(target, member);

            return null;
        }
    }
}
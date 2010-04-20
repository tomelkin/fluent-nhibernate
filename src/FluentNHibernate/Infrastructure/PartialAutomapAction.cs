using System;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Infrastructure
{
    public class PartialAutomapAction : IMappingAction
    {
        readonly ClassMapping mapping;

        public PartialAutomapAction(Type type)
            : this (new ClassMapping { Type = type })
        {}

        public PartialAutomapAction(ClassMapping mapping)
        {
            this.mapping = mapping;
        }

        public AutomappingTarget CreateTarget(IAutomappingInstructions mainInstructions)
        {
            return new AutomappingTarget(mapping.Type, mapping, new EntityAutomappingInstructions(mainInstructions));
        }
    }
}
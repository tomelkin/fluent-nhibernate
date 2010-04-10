using System;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Infrastructure
{
    public class PartialAutomapAction : IMappingAction
    {
        readonly ClassMapping mapping;
        readonly AutomappingEntitySetup setup;

        public PartialAutomapAction(Type type, AutomappingEntitySetup setup)
            : this (new ClassMapping { Type = type }, setup)
        {}

        public PartialAutomapAction(ClassMapping mapping, AutomappingEntitySetup setup)
        {
            this.mapping = mapping;
            this.setup = setup;
        }

        public AutomappingTarget CreateTarget(IAutomappingInstructions mainInstructions)
        {
            var instructions = new EntityAutomappingInstructions(mainInstructions, setup);

            return new AutomappingTarget(mapping.Type, mapping, instructions);
        }
    }
}
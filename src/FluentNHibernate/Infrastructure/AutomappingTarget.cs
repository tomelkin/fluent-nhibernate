using System;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Infrastructure
{
    public class AutomappingTarget
    {
        public Type Type { get; private set; }
        public ITopMapping Mapping { get; private set; }
        public IAutomappingInstructions Instructions { get; private set; }

        public AutomappingTarget(Type type, ITopMapping mapping, IAutomappingInstructions instructions)
        {
            Type = type;
            Mapping = mapping;
            Instructions = instructions;
        }
    }
}
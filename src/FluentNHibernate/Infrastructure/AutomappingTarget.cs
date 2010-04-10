using System;
using System.Linq;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Infrastructure
{
    public class AutomappingTarget
    {
        public Type Type { get; private set; }
        public ITopMapping Mapping { get; private set; }
        public IEntityAutomappingInstructions Instructions { get; private set; }

        public AutomappingTarget(Type type, ITopMapping mapping, IEntityAutomappingInstructions instructions)
        {
            Type = type;
            Mapping = mapping;
            Instructions = instructions;
        }

        public bool IsMemberUsed(Member member)
        {
            return Mapping.GetUsedMembers()
                .Contains(member);
        }
    }
}
using System.Collections.Generic;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping.Steps
{
    public class ComponentStep : IAutomappingStep
    {
        private readonly IAutomappingConfiguration cfg;
        private readonly IAutomapper mapper;

        public ComponentStep(IAutomappingConfiguration cfg, IAutomapper mapper)
        {
            this.cfg = cfg;
            this.mapper = mapper;
        }

        public bool ShouldMap(AutomappingTarget target, Member member)
        {
            return cfg.IsComponent(member.PropertyType);
        }

        public IMemberMapping Map(AutomappingTarget target, Member member)
        {
            var mapping = new ComponentMapping(ComponentType.Component)
            {
                Name = member.Name,
                Member = member,
                //ContainingEntityType = classMap.Type,
                Type = member.PropertyType
            };

            //mapper.FlagAsMapped(property.PropertyType);
            //mapper.MergeMap(property.PropertyType, mapping, new List<Member>());

            return mapping;
        }
    }
}
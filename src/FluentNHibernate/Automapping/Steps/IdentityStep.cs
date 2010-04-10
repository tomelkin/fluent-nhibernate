using System;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Identity;

namespace FluentNHibernate.Automapping.Steps
{
    public class IdentityStep : IAutomappingStep
    {
        private readonly IAutomappingConfiguration cfg;

        public IdentityStep(IAutomappingConfiguration cfg)
        {
            this.cfg = cfg;
        }

        public bool ShouldMap(AutomappingTarget target, Member member)
        {
            return cfg.IsId(member) && !target.Mapping.HasId();
        }

        public IMemberMapping Map(AutomappingTarget target, Member member)
        {
            if (!(target.Mapping is ClassMapping)) return null;

            var idMapping = new IdMapping { /*ContainingEntityType = classMap.Type*/ };
            idMapping.AddDefaultColumn(new ColumnMapping() { Name = member.Name });
            idMapping.Name = member.Name;
            idMapping.Type = new TypeReference(member.PropertyType);
            idMapping.Member = member;
            idMapping.SetDefaultValue("Generator", GetDefaultGenerator(member));

            return idMapping;
        }

        private GeneratorMapping GetDefaultGenerator(Member property)
        {
            var generatorMapping = new GeneratorMapping();
            var defaultGenerator = new GeneratorBuilder(generatorMapping, property.PropertyType);

            if (property.PropertyType == typeof(Guid))
                defaultGenerator.GuidComb();
            else if (property.PropertyType == typeof(int) || property.PropertyType == typeof(long))
                defaultGenerator.Identity();
            else
                defaultGenerator.Assigned();

            return generatorMapping;
        }
    }
}
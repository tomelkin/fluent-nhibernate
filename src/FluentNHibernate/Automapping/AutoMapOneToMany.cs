using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping
{
    public class AutoMapOneToMany : IAutoMapper
    {
        readonly AutoSimpleTypeCollection simpleTypeCollectionStep;
        readonly AutoEntityCollection entityCollectionStep;

        public AutoMapOneToMany(IAutomappingConfiguration cfg)
        {
            simpleTypeCollectionStep = new AutoSimpleTypeCollection(cfg);
            entityCollectionStep = new AutoEntityCollection(cfg);
        }

        public bool ShouldMap(Member member)
        {
            return simpleTypeCollectionStep.ShouldMap(member) ||
                   entityCollectionStep.ShouldMap(member);
        }

        public void Map(ClassMappingBase classMap, Member property)
        {
            if (property.DeclaringType != classMap.Type)
                return;

            if (simpleTypeCollectionStep.ShouldMap(property))
                simpleTypeCollectionStep.Map(classMap, property);
            else if (entityCollectionStep.ShouldMap(property))
                entityCollectionStep.Map(classMap, property);
        }
    }
}

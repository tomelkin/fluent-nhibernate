using FluentNHibernate.Infrastructure;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Automapping.Steps
{
    public class CollectionStep : IAutomappingStep
    {
        readonly IAutomappingConfiguration cfg;
        readonly AutoKeyMapper keys;
        AutoCollectionCreator collections;

        public CollectionStep(IAutomappingConfiguration cfg)
        {
            this.cfg = cfg;
            keys = new AutoKeyMapper(cfg);
            collections = new AutoCollectionCreator();
        }

        public bool ShouldMap(AutomappingTarget target, Member member)
        {
            return member.CanWrite &&
                member.PropertyType.Namespace.In("System.Collections.Generic", "Iesi.Collections.Generic");
        }

        public IMemberMapping Map(AutomappingTarget target, Member member)
        {
            //if (property.DeclaringType != classMap.Type)
            //    return;

            var mapping = collections.CreateCollectionMapping(member.PropertyType);

            //mapping.ContainingEntityType = classMap.Type;
            mapping.Member = member;
            mapping.SetDefaultValue(x => x.Name, member.Name);

            //SetRelationship(property, classMap, mapping);
            //keys.SetKey(property, classMap, mapping);

            return mapping;
        }

        private void SetRelationship(Member property, ClassMappingBase classMap, ICollectionMapping mapping)
        {
            var relationship = new OneToManyMapping
            {
                Class = new TypeReference(property.PropertyType.GetGenericArguments()[0]),
                ContainingEntityType = classMap.Type
            };

            mapping.SetDefaultValue(x => x.Relationship, relationship);
        }
    }
}
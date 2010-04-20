using System;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping.Steps
{
    public class HasManyStep : IAutomappingStep
    {
        readonly SimpleTypeCollectionStep simpleTypeCollectionStepStep;
        readonly CollectionStep collectionStep;

        public HasManyStep(IAutomappingConfiguration cfg)
        {
            simpleTypeCollectionStepStep = new SimpleTypeCollectionStep(cfg);
            collectionStep = new CollectionStep(cfg);
        }

        public bool ShouldMap(AutomappingTarget target, Member member)
        {
            return simpleTypeCollectionStepStep.ShouldMap(target, member) ||
                   collectionStep.ShouldMap(target, member);
        }

        public IMemberMapping Map(AutomappingTarget target, Member member)
        {
            //if (property.DeclaringType != classMap.Type)
            //    return;

            if (simpleTypeCollectionStepStep.ShouldMap(target, member))
                return simpleTypeCollectionStepStep.Map(target, member);
            if (collectionStep.ShouldMap(target, member))
                return collectionStep.Map(target, member);

            throw new InvalidOperationException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Visitors
{
    public class SeparateSubclassVisitor : DefaultMappingModelVisitor
    {
        List<ClassMapping> classes;
        List<SubclassMapping> subclasses;
        HibernateMapping currentHibernateMapping;

        public override void ProcessHibernateMapping(HibernateMapping hibernateMapping)
        {
            currentHibernateMapping = hibernateMapping;
            classes = hibernateMapping.Classes.ToList();
            subclasses = hibernateMapping.Subclasses.ToList();

            subclasses.Each(PairSubclass);
            classes.Each(PairClass);
        }

        void PairClass(ClassMapping mapping)
        {
            var closestSubclasses = FindClosestSubclasses(mapping.Type);

            foreach (var subclass in closestSubclasses)
            {
                subclass.ChangeSubclassType(CreateSubclass(mapping));

                mapping.AddSubclass(subclass);
                currentHibernateMapping.RemoveSubclass(subclass);
            }
        }

        void PairSubclass(SubclassMapping mapping)
        {
            var closestSubclasses = FindClosestSubclasses(mapping.Type);

            foreach (var subclass in closestSubclasses)
            {
                subclass.ChangeSubclassType(mapping.SubclassType);

                mapping.AddSubclass(subclass);
                currentHibernateMapping.RemoveSubclass(subclass);
            }
        }

        IEnumerable<SubclassMapping> FindClosestSubclasses(Type type)
        {
            var extendsSubclasses = subclasses
                .Where(x => x.Extends == type);        
            var distanceSubclasses = SortByDistanceFrom(type, subclasses.Except(extendsSubclasses));

            if (distanceSubclasses.Keys.Count == 0 && !extendsSubclasses.Any())
                return new SubclassMapping[0];
            if (distanceSubclasses.Keys.Count == 0)
                return extendsSubclasses;

            var lowestDistance = distanceSubclasses.Keys.Min();

            return distanceSubclasses[lowestDistance].Concat(extendsSubclasses);
        }

        SubclassType CreateSubclass(ClassMapping mapping)
        {
            return mapping.Discriminator == null ? SubclassType.JoinedSubclass : SubclassType.Subclass;
        }

        bool IsMapped(Type type, IEnumerable<SubclassMapping> providers)
        {
            return providers.Any(x => x.Type == type);
        }

        /// <summary>
        /// Takes a type that represents the level in the class/subclass-hiearchy that we're starting from, the parent,
        /// this can be a class or subclass; also takes a list of subclass providers. The providers are then iterated
        /// and added to a dictionary key'd by the types "distance" from the parentType; distance being the number of levels
        /// between parentType and the subclass-type.
        /// 
        /// By default if the Parent type is an interface the level will always be zero. At this time there is no check for
        /// hierarchical interface inheritance.
        /// </summary>
        /// <param name="parentType">Starting point, parent type.</param>
        /// <param name="availableSubclasses">List of subclasses</param>
        /// <returns>Dictionary key'd by the distance from the parentType.</returns>
        IDictionary<int, IList<SubclassMapping>> SortByDistanceFrom(Type parentType, IEnumerable<SubclassMapping> availableSubclasses)
        {
            var arranged = new Dictionary<int, IList<SubclassMapping>>();

            foreach (var subclass in availableSubclasses)
            {
                var subclassType = subclass.Type;
                var level = 0;

                bool implOfParent = parentType.IsInterface
                    ? DistanceFromParentInterface(parentType, subclassType, ref level)
                    : DistanceFromParentBase(parentType, subclassType.BaseType, ref level);

                if (!implOfParent) continue;

                if (!arranged.ContainsKey(level))
                    arranged[level] = new List<SubclassMapping>();

                arranged[level].Add(subclass);
            }

            return arranged;
        }

        /// <summary>
        /// The evalType starts out as the original subclass. The class hiearchy is only
        /// walked if the subclass inherits from a class that is included in the subclassProviders.
        /// </summary>
        /// <param name="parentType"></param>
        /// <param name="evalType"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        bool DistanceFromParentInterface(Type parentType, Type evalType, ref int level)
        {
            if (!evalType.HasInterface(parentType)) return false;

            if (!(evalType == typeof(object)) &&
                IsMapped(evalType.BaseType, subclasses))
            {
                //Walk the tree if the subclasses base class is also in the subclassProviders
                level++;
                DistanceFromParentInterface(parentType, evalType.BaseType, ref level);
            }

            return true;
        }

        /// <summary>
        /// The evalType is always one class higher in the hiearchy starting from the original subclass. The class 
        /// hiearchy is walked until the IsTopLevel (base class is Object) is met. The level is only incremented if 
        /// the subclass inherits from a class that is also in the subclassProviders.
        /// </summary>
        /// <param name="parentType"></param>
        /// <param name="evalType"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        bool DistanceFromParentBase(Type parentType, Type evalType, ref int level)
        {
            var evalImplementsParent = false;
            if (evalType == parentType)
                evalImplementsParent = true;

            if (!evalImplementsParent && !(evalType == typeof(object)))
            {
                //If the eval class does not inherit the parent but it is included
                //in the subclassprovides, then the original subclass can not inherit 
                //directly from the parent.
                if (IsMapped(evalType, subclasses))
                    level++;
                evalImplementsParent = DistanceFromParentBase(parentType, evalType.BaseType, ref level);
            }

            return evalImplementsParent;
        }
    }
}
using System;

namespace FluentNHibernate.Automapping
{
    public interface IAutomappingConfiguration
    {
        bool ShouldMap(Member member);
        bool IsId(Member member);
        Type GetParentSideForManyToMany(Type left, Type right);
        bool IsConcreteBaseType(Type type);
        bool IsComponent(Type type);
        string GetComponentColumnPrefix(Member member);
        bool IsDiscriminated(Type type);
        string GetDiscriminatorColumn(Type type);
        SubclassStrategy GetSubclassStrategy(Type type);
        bool AbstractClassIsLayerSupertype(Type type);
        string SimpleTypeCollectionValueColumn(Member member);
    }
}
using System;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IIndeterminateSubclassMappingProvider : IProvider
    {
        Type EntityType { get; }
        Type Extends { get; }
    }
}
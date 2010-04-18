using System;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Mapping.Providers
{
    public interface IExternalComponentMappingProvider : IProvider
    {
        Type Type { get; }
        ExternalComponentMapping GetComponentMapping();
    }
}
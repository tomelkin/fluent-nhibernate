using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.MappingModel;

namespace FluentNHibernate.Mapping
{
    public interface IFilterDefinition : IProvider
    {
        string Name { get; }
        FilterDefinitionMapping GetFilterMapping();
    }
}

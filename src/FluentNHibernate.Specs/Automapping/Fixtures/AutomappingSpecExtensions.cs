using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Specs.Automapping.Fixtures
{
    public static class AutomappingSpecExtensions
    {
        public static ClassMapping BuildMappingFor<T>(this AutoPersistenceModel model)
        {
            return model.BuildMappings()
                .SelectMany(x => x.Classes)
                .FirstOrDefault(x => x.Type == typeof(T));
        }
    }
}
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping.Steps
{
    public static class StepExtensions
    {
        public static bool HasId(this ITopMapping mapping)
        {
            if (!(mapping is ClassMapping))
                return false;

            return ((ClassMapping)mapping).Id != null;
        }
    }
}
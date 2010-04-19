using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Infrastructure
{
    public interface IProvider
    {
        ITopMapping GetMapping();
    }
}
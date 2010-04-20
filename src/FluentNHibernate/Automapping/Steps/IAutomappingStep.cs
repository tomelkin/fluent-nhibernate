using FluentNHibernate.Infrastructure;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Automapping.Steps
{
    public interface IAutomappingStep
    {
        bool ShouldMap(AutomappingTarget target, Member member);
        IMemberMapping Map(AutomappingTarget target, Member member);
    }
}
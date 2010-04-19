using FluentNHibernate.Automapping;

namespace FluentNHibernate
{
    public class AutomappingInstructions : IAutomappingInstructions
    {
        public IAutomappingConfiguration Configuration { get; set; }
    }

    public interface IAutomappingInstructions
    {
        IAutomappingConfiguration Configuration { get; }
    }
}
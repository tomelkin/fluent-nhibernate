using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Infrastructure
{
    public class ManualAction : IMappingAction
    {
        readonly ITopMapping mapping;

        public ManualAction(ITopMapping mapping)
        {
            this.mapping = mapping;
        }

        public ITopMapping GetMapping()
        {
            return mapping;
        }

        public override string ToString()
        {
            return "{ ManualAction: " + mapping.GetType().Name + "<" + mapping.Type.Name + "> }";
        }
    }
}
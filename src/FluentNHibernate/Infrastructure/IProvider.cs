using System;
using FluentNHibernate.MappingModel.ClassBased;

namespace FluentNHibernate.Infrastructure
{
    public interface IProvider
    {
        IMappingAction GetAction();
    }

    public interface IMappingAction
    {
        ITopMapping Execute(IMappingCompiler compiler);
    }

    public class AutomapAction : IMappingAction
    {
        readonly ITopMapping mapping;

        public AutomapAction(ITopMapping mapping)
        {
            this.mapping = mapping;
        }

        public ITopMapping Execute(IMappingCompiler compiler)
        {
            return compiler.AutoMap(mapping);
        }
    }

    public class ManualAction : IMappingAction
    {
        readonly ITopMapping mapping;

        public ManualAction(ITopMapping mapping)
        {
            this.mapping = mapping;
        }

        public ITopMapping Execute(IMappingCompiler compiler)
        {
            return compiler.ManualMap(mapping);
        }
    }
}
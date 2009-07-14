using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;

namespace FluentNHibernate.Conventions.Instances
{
    public interface IClassInstance : IClassInspector
    {
        IClassInstance Not { get; }
        new IOptimisticLockBuilder OptimisticLock { get; }
        void WithTable(string tableName);
        void DynamicInsert();
        void DynamicUpdate();
        void BatchSize(int size);
        void LazyLoad();
        void ReadOnly();
        void Schema(string schema);
    }
}
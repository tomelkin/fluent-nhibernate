using System;
using System.Collections.Generic;

namespace FluentNHibernate.MappingModel.ClassBased
{
    public interface ITopMapping
    {
        void AddTo(HibernateMapping hbm);
        IEnumerable<Member> GetUsedMembers();
        Type Type { get; }
        void AddMappedMember(IMemberMapping mapping);
    }
}
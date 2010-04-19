using System;
using System.Collections.Generic;
using FluentNHibernate.Infrastructure;

namespace FluentNHibernate.Testing
{
    internal class EmptySource : ITypeSource
    {
        public IEnumerable<Type> GetTypes()
        {
            return new Type[0];
        }
    }
}
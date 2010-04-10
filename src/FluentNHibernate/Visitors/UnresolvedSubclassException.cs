using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Visitors
{
    [Serializable]
    public class UnresolvedSubclassException : Exception
    {
        public Type[] UnresolvedTypes { get; private set; }

        public UnresolvedSubclassException(string message)
            : base(message)
        {}

        void SetTypes(IEnumerable<Type> types)
        {
            UnresolvedTypes = types.ToArray();
        }

        public static UnresolvedSubclassException New(IEnumerable<Type> types)
        {
            var sb = new StringBuilder();

            sb.Append("Unresolved subclasses found: <");

            types.Select(x => x.Name)
                .Each(x =>
                {
                    sb.Append((string)x);
                    sb.Append(", ");
                });

            sb.Length -= 2;
            sb.Append(">");

            var ex = new UnresolvedSubclassException(sb.ToString());
            ex.SetTypes(types);

            return ex;
        }
    }
}
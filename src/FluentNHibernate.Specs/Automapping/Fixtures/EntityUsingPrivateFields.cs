using System;

namespace FluentNHibernate.Specs.Automapping.Fixtures
{
    class EntityUsingPrivateFields
    {
        int id;
        string one;
        DateTime two;
        DateTime? three;

        public string PublicPropertyThatShouldBeIgnored { get; set; }
    }
}
using System;

namespace FluentNHibernate.Specs.FluentInterface.Fixtures
{
    class EntityWithFieldsParent : EntityParent
    {}

    class EntityWithFields : EntityWithFieldsParent
    {
        public string Name;
    }
}
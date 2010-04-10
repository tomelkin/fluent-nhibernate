using System;

namespace FluentNHibernate.Specs.FluentInterface.Fixtures
{
    class EntityWithPropertiesParent : EntityParent
    {}

    class EntityWithProperties : EntityWithPropertiesParent
    {
        public string Name { get; set; }
    }

    class EntityWithPrivatePropertiesParent : EntityParent
    {}

    class EntityWithPrivateProperties : EntityWithPrivatePropertiesParent
    {
        private string Name { get; set; }
    }

    class ProxyClass
    {}

    class PersisterClass
    {}
}

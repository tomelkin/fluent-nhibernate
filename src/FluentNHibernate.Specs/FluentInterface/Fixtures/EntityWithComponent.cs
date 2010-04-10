using System;
using System.Collections;

namespace FluentNHibernate.Specs.FluentInterface.Fixtures
{
    class EntityWithComponentParent : EntityParent
    {}

    class EntityWithComponent : EntityWithComponentParent
    {
        public ComponentTarget Component { get; set; }
        public IDictionary DynamicComponent { get; set; }
    }

    class EntityWithFieldComponentParent : EntityParent
    {}

    class EntityWithFieldComponent : EntityWithFieldComponentParent
    {
        public ComponentTarget Component;
        public IDictionary DynamicComponent;
    }

    class ComponentTarget
    {
    }
}
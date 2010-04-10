using System;

namespace FluentNHibernate.Specs.FluentInterface.Fixtures
{
    class EntityWithReferencesParent : EntityParent
    {}

    class EntityWithReferences : EntityWithReferencesParent
    {
        public ReferenceTarget Reference { get; set; }
    }

    class ReferenceTarget
    {}
}
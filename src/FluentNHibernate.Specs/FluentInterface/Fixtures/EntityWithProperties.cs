using System;

namespace FluentNHibernate.Specs.FluentInterface.Fixtures
{
    class EntityWithProperties
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    class EntityWithPrivateProperties
    {
        private int Id { get; set; }
        private string Name { get; set; }
    }
}

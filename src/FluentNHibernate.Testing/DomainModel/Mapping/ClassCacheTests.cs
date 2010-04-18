using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ClassCacheTests
    {
        [Test]
        public void ShouldBeFirstElement()
        {
            new MappingTester<CacheTarget>()
                .ForMapping(c =>
                {
                    c.Id(x => x.Id);
                    c.Map(x => x.Name);
                    c.Cache.ReadWrite();
                })
                .Element("class/*[1]").HasName("cache");
        }

        [Test]
        public void ShouldCreateCacheElement()
        {
            new MappingTester<CacheTarget>()
                .ForMapping(c =>
                {
                    c.Id(x => x.Id);
                    c.Cache.ReadWrite();
                })
                .Element("class/cache").Exists();
        }

        [Test]
        public void ShouldOutputReadWriteForAsReadWrite()
        {
            new MappingTester<CacheTarget>()
                .ForMapping(c =>
                {
                    c.Id(x => x.Id);
                    c.Cache.ReadWrite();
                })
                .Element("class/cache").HasAttribute("usage", "read-write");
        }

        [Test]
        public void ShouldOutputNonstrictReadWriteForAsNonStrictReadWrite()
        {
            new MappingTester<CacheTarget>()
                .ForMapping(c =>
                {
                    c.Id(x => x.Id);
                    c.Cache.NonStrictReadWrite();
                })
                .Element("class/cache").HasAttribute("usage", "nonstrict-read-write");
        }

        [Test]
        public void ShouldOutputNonstrictReadWriteForAsReadOnly()
        {
            new MappingTester<CacheTarget>()
                .ForMapping(c =>
                {
                    c.Id(x => x.Id);
                    c.Cache.ReadOnly();
                })
                .Element("class/cache").HasAttribute("usage", "read-only");
        }

        [Test]
        public void ShouldAllowAnythingForAsCustom()
        {
            new MappingTester<CacheTarget>()
                .ForMapping(c =>
                {
                    c.Id(x => x.Id);
                    c.Cache.CustomUsage("something-else");
                })
                .Element("class/cache").HasAttribute("usage", "something-else");
        }

        [Test]
        public void ShouldWriteRegionWhenAssigned()
        {
            new MappingTester<CacheTarget>()
                .ForMapping(c =>
                {
                    c.Id(x => x.Id);
                    c.Cache.ReadWrite().Region("MyRegion");
                })
                .Element("class/cache").HasAttribute("region", "MyRegion");
        }

        private class CacheTarget
        {
            public virtual int Id { get; set; }
            public virtual string Name { get; set; }
        }
    }
}
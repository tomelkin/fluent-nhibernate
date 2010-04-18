using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ClassMapDynamicUpdateTester
    {
        [Test]
        public void CanOverrideDynamicUpdate()
        {
            new MappingTester<MappedObject>()
                .ForMapping(c =>
                {
                    c.Id(x => x.Id);
                    c.DynamicUpdate();
                })
                .Element("class").HasAttribute("dynamic-update", "true");
        }

        [Test]
        public void CanOverrideNoDynamicUpdate()
        {
            new MappingTester<MappedObject>()
                .ForMapping(c =>
                {
                    c.Id(x => x.Id);
                    c.Not.DynamicUpdate();
                })
                .Element("class").HasAttribute("dynamic-update", "false");
        }
    }
}
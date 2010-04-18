using NUnit.Framework;

namespace FluentNHibernate.Testing.DomainModel.Mapping
{
    [TestFixture]
    public class ClassMapDynamicInsertTester
    {
        [Test]
        public void CanOverrideDynamicInsert()
        {
            new MappingTester<MappedObject>()
                .ForMapping(c =>
                {
                    c.Id(x => x.Id);
                    c.DynamicInsert();
                })
                .Element("class").HasAttribute("dynamic-insert", "true");
        }

        [Test]
        public void CanOverrideNoDynamicInsert()
        {
            new MappingTester<MappedObject>()
                .ForMapping(c =>
                {
                    c.Id(x => x.Id);
                    c.Not.DynamicInsert();
                })
                .Element("class").HasAttribute("dynamic-insert", "false");
        }
    }
}
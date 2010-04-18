using System;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class MultipleHelperConventions
    {
        [Test]
        public void AlwaysShouldSetDefaultLazyToTrue()
        {
            var classMap = new ClassMap<Target>();
            classMap.Id(x => x.Id);
            
            var conventions = new ConventionsCollection {DefaultLazy.Always(), DefaultCascade.All()};

            var instructions = new PersistenceInstructions();
            instructions.AddSource(new StubProviderSource(classMap));
            instructions.UseConventions(conventions);

            var mapping = instructions.BuildMappings().First();

            mapping.DefaultLazy.ShouldBeTrue();
            mapping.DefaultCascade.ShouldEqual("all");
        }

        private class Target
        {
            public int Id { get; set; }
        }
    }
}
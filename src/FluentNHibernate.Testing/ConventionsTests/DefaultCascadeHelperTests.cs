using System;
using System.Linq;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.Mapping;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests
{
    [TestFixture]
    public class DefaultCascadeHelperTests
    {
        [Test]
        public void ShouldSetDefaultAccessToValue()
        {
            var classMap = new ClassMap<Target>();
            classMap.Id(x => x.Id);
            
            var conventions = new ConventionsCollection {DefaultCascade.All()};

            var instructions = new PersistenceInstructions();
            instructions.AddSource(new StubProviderSource(classMap));
            instructions.UseConventions(conventions);

            instructions.BuildMappings()
                .First()
                .DefaultCascade.ShouldEqual("all");
        }

        private class Target
        {
            public int Id { get; set; }
        }
    }
}
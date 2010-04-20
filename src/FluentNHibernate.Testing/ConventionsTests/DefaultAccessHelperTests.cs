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
    public class DefaultAccessHelperTests
    {
        [Test]
        public void ShouldSetDefaultAccessToValue()
        {
            var classMap = new ClassMap<Target>();
            classMap.Id(x => x.Id);
            
            var conventions = new ConventionsCollection {DefaultAccess.Field()};
            var instructions = new PersistenceInstructions();
            instructions.AddActions(classMap);
            instructions.UseConventions(conventions);

            instructions.BuildMappings()
                .First()
                .DefaultAccess.ShouldEqual("field");
        }

        private class Target
        {
            public int Id { get; set; }
        }
    }
}
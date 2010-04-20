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
    public class DefaultLazyHelperTests
    {
        [Test]
        public void AlwaysShouldSetDefaultLazyToTrue()
        {
            var classMap = new ClassMap<Target>();
            classMap.Id(x => x.Id);
            
            var conventions = new ConventionsCollection {DefaultLazy.Always()};

            var instructions = new PersistenceInstructions();
            instructions.AddActions(classMap);
            instructions.UseConventions(conventions);

            instructions.BuildMappings()
                .First()
                .DefaultLazy.ShouldBeTrue();
        }

        [Test]
        public void NeverShouldSetDefaultLazyToFalse()
        {
            var classMap = new ClassMap<Target>();
            classMap.Id(x => x.Id);
            
            var conventions = new ConventionsCollection {DefaultLazy.Never()};

            var instructions = new PersistenceInstructions();
            instructions.AddActions(classMap);
            instructions.UseConventions(conventions);

            instructions.BuildMappings()
                .First()
                .DefaultLazy.ShouldBeFalse();
        }

        private class Target
        {
            public int Id { get; set; }
        }
    }
}
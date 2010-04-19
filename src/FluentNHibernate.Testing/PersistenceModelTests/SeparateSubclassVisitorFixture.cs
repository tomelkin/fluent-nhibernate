using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using FluentNHibernate.Mapping.Providers;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Visitors;
using NUnit.Framework;

namespace FluentNHibernate.Testing.PersistenceModelTests
{
    [TestFixture]
    public class SeparateSubclassVisitorFixture
    {
        [Test]
        public void Should_add_subclass_that_implements_the_parent_interface()
        {
            /* The Parent is the IFoo interface the desired results 
             * of this test is the inclusion of the Foo<T> through the
             * GenericFooMap<T> subclass mapping.
             */
            var mappings = GetMappingsFor(new FooMap(), new StringFooMap());
            var fooMapping = mappings.SelectMany(x => x.Classes).First(x => x.Type == typeof(IFoo));

            var sut = CreateSut();
            sut.ProcessHibernateMapping(mappings.First());
            Assert.AreEqual(1, fooMapping.Subclasses.Count());
            Assert.AreEqual(1, fooMapping.Subclasses.Where(sub => sub.Type.Equals(typeof(Foo<string>))).Count());
        }

        [Test]
        public void Should_add_subclass_that_implements_the_parent_base()
        {
            /* The Parent is the FooBase class the desired results 
             * of this test is the inclusion of the Foo<T> through the
             * GenericFooMap<T> subclass mapping.
             */
            var mappings = GetMappingsFor(new BaseMap(), new StringFooMap());
            var fooMapping = mappings.SelectMany(x => x.Classes).First(x => x.Type == typeof(Base));

            var sut = CreateSut();
            sut.ProcessHibernateMapping(mappings.First());
            Assert.AreEqual(1, fooMapping.Subclasses.Count());
            Assert.AreEqual(1, fooMapping.Subclasses.Where(sub => sub.Type.Equals(typeof(Foo<string>))).Count());
        }

        [Test]
        public void Should_not_add_subclassmap_that_does_not_implement_parent_interface()
        {
            /* The Parent is the IFoo interface the desired results 
             * of this test is the exclusion of the StandAlone class 
             * since it does not implement the interface.
             */
            var mappings = GetMappingsFor(new FooMap(), new StandAloneMap());
            var fooMapping = mappings.SelectMany(x => x.Classes).First(x => x.Type == typeof(IFoo));
            
            var sut = CreateSut();
            sut.ProcessHibernateMapping(mappings.First());
            Assert.AreEqual(0, fooMapping.Subclasses.Count());
        }

        [Test]
        public void Should_not_add_subclassmap_that_does_not_implement_parent_base()
        {
            /* The Parent is the FooBase class the desired results 
             * of this test is the exclusion of the StandAlone class 
             * since it does not implement the interface.
             */
            var mappings = GetMappingsFor(new BaseMap(), new StandAloneMap());
            var fooMapping = mappings.SelectMany(x => x.Classes).First(x => x.Type == typeof(Base));
            
            var sut = CreateSut();
            sut.ProcessHibernateMapping(mappings.First());
            Assert.AreEqual(0, fooMapping.Subclasses.Count());
        }

        [Test]
        public void Should_not_add_subclassmap_that_implements_a_subclass_of_the_parent_interface()
        {
            /* The Parent is the IFoo interface the desired results 
             * of this test is the inclusion of the BaseImpl class and 
             * the exclusion of the Foo<T> class since it implements 
             * the BaseImpl class which already implements FooBase.
             */
            var mappings = GetMappingsFor(new FooMap(), new BaseImplMap(), new StringFooMap());
            var fooMapping = mappings.SelectMany(x => x.Classes).First(x => x.Type == typeof(IFoo));
            
            var sut = CreateSut();
            sut.ProcessHibernateMapping(mappings.First());
            Assert.AreEqual(1, fooMapping.Subclasses.Count());
            Assert.AreEqual(1, fooMapping.Subclasses.Where(sub => sub.Type.Equals(typeof(BaseImpl))).Count());
        }

        [Test]
        public void Should_not_add_subclassmap_that_implements_a_subclass_of_the_parent_base()
        {
            /* The Parent is the FooBase class the desired results 
             * of this test is the inclusion of the BaseImpl class and 
             * the exclusion of the Foo<T> class since it implements 
             * the BaseImpl class which already implements FooBase.
             */
            var mappings = GetMappingsFor(new BaseMap(), new BaseImplMap(), new StringFooMap());
            var fooMapping = mappings.SelectMany(x => x.Classes).First(x => x.Type == typeof(Base));

            var sut = CreateSut();
            sut.ProcessHibernateMapping(mappings.First());
            Assert.AreEqual(1, fooMapping.Subclasses.Count());
            Assert.AreEqual(1, fooMapping.Subclasses.Where(sub => sub.Type.Equals(typeof(BaseImpl))).Count());
        }

        [Test]
        public void Should_add_explicit_extend_subclasses_to_their_parent()
        {
            var mappings = GetMappingsFor(new ExtendsParentMap(), new ExtendsChildMap());
            var fooMapping = mappings.SelectMany(x => x.Classes).First(x => x.Type == typeof(ExtendsParent));

            var sut = CreateSut();
            sut.ProcessHibernateMapping(mappings.First());
            Assert.AreEqual(1, fooMapping.Subclasses.Count());
            Assert.AreEqual(1, fooMapping.Subclasses.Where(sub => sub.Type.Equals(typeof(ExtendsChild))).Count());
        }

        private SeparateSubclassVisitor CreateSut()
        {
            return new SeparateSubclassVisitor();
        }

        IEnumerable<HibernateMapping> GetMappingsFor(params IProvider[] providers)
        {
            var instructions = new PersistenceInstructions();

            instructions.AddSource(new StubProviderSource(providers));

            return instructions.BuildMappings();
        }

        private interface IFoo
        {
            int Id { get; }
        }

        private class Base : IFoo
        {
            public int Id { get; set; }
        }

        private abstract class BaseImpl : Base
        { }

        private class Foo<T> : BaseImpl, IFoo
        { }

        private class FooMap : ClassMap<IFoo>
        {
            public FooMap()
            {
                Id(x => x.Id);
            }
        }

        private class BaseMap : ClassMap<Base>
        {
            public BaseMap()
            {
                Id(x => x.Id);
            }
        }

        private class BaseImplMap : SubclassMap<BaseImpl>
        { }

        private abstract class GenericFooMap<T> : SubclassMap<Foo<T>>
        { }

        private class StringFooMap : GenericFooMap<string>
        { }


        private interface IStand
        { }

        private class StandAlone : IStand
        { }

        private class StandAloneMap : SubclassMap<StandAlone>
        { }

        class ExtendsParent
        {
            public int Id { get; set; }
        }

        class ExtendsChild
        {}

        class ExtendsParentMap : ClassMap<ExtendsParent>
        {
            public ExtendsParentMap()
            {
                Id(x => x.Id);
            }
        }

        class ExtendsChildMap : SubclassMap<ExtendsChild>
        {
            public ExtendsChildMap()
            {
                Extends<ExtendsParent>();
            }
        }
    }
}

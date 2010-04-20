using System;
using System.Linq;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.OverridingFluentInterface
{
    [TestFixture]
    public class SubclassConventionTests
    {
        private IProvider mapping;
        private Type mappingType;
        SubclassMap<ExampleInheritedClass> subclassMap;
        ConventionsCollection conventions;

        [SetUp]
        public void CreatePersistenceModel()
        {
            conventions = new ConventionsCollection();
        }

        [Test]
        public void AbstractShouldntBeOverwritten()
        {
            Mapping(x => x.Abstract());

            Convention(x => x.Not.Abstract());

            VerifyModel(x => x.Abstract.ShouldBeTrue());
        }

        [Test]
        public void DynamicInsertShouldntBeOverwritten()
        {
            Mapping(x => x.DynamicInsert());

            Convention(x => x.Not.DynamicInsert());

            VerifyModel(x => x.DynamicInsert.ShouldBeTrue());
        }

        [Test]
        public void DynamicUpdateShouldntBeOverwritten()
        {
            Mapping(x => x.DynamicUpdate());

            Convention(x => x.Not.DynamicUpdate());

            VerifyModel(x => x.DynamicUpdate.ShouldBeTrue());
        }

        [Test]
        public void LazyLoadShouldntBeOverwritten()
        {
            Mapping(x => x.LazyLoad());

            Convention(x => x.Not.LazyLoad());

            VerifyModel(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void ProxyShouldntBeOverwritten()
        {
            Mapping(x => x.Proxy(typeof(int)));

            Convention(x => x.Proxy(typeof(string)));

            VerifyModel(x => x.Proxy.ShouldEqual(typeof(int).AssemblyQualifiedName));
        }

        [Test]
        public void SelectBeforeUpdateShouldntBeOverwritten()
        {
            Mapping(x => x.SelectBeforeUpdate());

            Convention(x => x.Not.SelectBeforeUpdate());

            VerifyModel(x => x.SelectBeforeUpdate.ShouldBeTrue());
        }

        [Test]
        public void ExtendsShouldntBeOverwritten()
        {
            Mapping(x => x.Extends<ExampleClass>());

            Convention(x => x.Extends<ExampleParentClass>());

            VerifyModel(x => x.Extends.ShouldEqual(typeof(ExampleClass)));
        }

        #region Helpers

        private void Convention(Action<ISubclassInstance> convention)
        {
            conventions.Add(new SubclassConventionBuilder().Always(convention));
        }

        private void Mapping(Action<SubclassMap<ExampleInheritedClass>> mappingDefinition)
        {
            var classMap = new ClassMap<ExampleClass>();
            classMap.Id(x => x.Id);
            classMap.DiscriminateSubClassesOnColumn("col");

            subclassMap = new SubclassMap<ExampleInheritedClass>();

            mappingDefinition(subclassMap);

            mapping = classMap;
            mappingType = typeof(ExampleClass);
        }

        private void VerifyModel(Action<SubclassMapping> modelVerification)
        {
            var instructions = new PersistenceInstructions();
            instructions.AddActions(mapping, subclassMap);
            instructions.UseConventions(conventions);

            var generatedModels = instructions.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == mappingType) != null)
                .Classes.First()
                .Subclasses.First();

            modelVerification((SubclassMapping)modelInstance);
        }

        #endregion
    }
}
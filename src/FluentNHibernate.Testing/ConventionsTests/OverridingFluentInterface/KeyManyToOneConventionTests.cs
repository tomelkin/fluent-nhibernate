using System;
using System.Linq;
using System.Linq.Expressions;
using FluentNHibernate.Automapping.TestFixtures;
using FluentNHibernate.Automapping.TestFixtures.CustomTypes;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Helpers.Builders;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.Identity;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.OverridingFluentInterface
{
    [TestFixture]
    public class KeyManyToOneConventionTests
    {
        private IProvider mapping;
        ConventionsCollection conventions;

        [SetUp]
        public void CreatePersistenceModel()
        {
            conventions = new ConventionsCollection();
        }

        [Test]
        public void AccessShouldntBeOveridden()
        {
            Mapping(x => x.Access.Property());

            Convention(x => x.Access.BackField());

            VerifyModel(x => x.Access.ShouldEqual("property"));
        }

        [Test]
        public void ForeignKeyShouldntBeOveridden() 
        {
            Mapping(x => x.ForeignKey("foo"));

            Convention(x => x.ForeignKey("bar"));

            VerifyModel(x => x.ForeignKey.ShouldEqual("foo"));
        }

        [Test]
        public void LazyShouldntBeOveridden() 
        {
            Mapping(x => x.Lazy());

            Convention(x => x.Not.Lazy());

            VerifyModel(x => x.Lazy.ShouldEqual(true));
        }

        [Test]
        public void NotFoundShouldntBeOveridden() 
        {
            Mapping(x => x.NotFound.Ignore());

            Convention(x => x.NotFound.Exception());

            VerifyModel(x => x.NotFound.ShouldEqual("ignore"));
        }

        #region Helpers

        private void Convention(Action<IKeyManyToOneInstance> convention)
        {
            conventions.Add(new KeyManyToOneConventionBuilder().Always(convention));
        }

        private void Mapping(Action<KeyManyToOnePart> mappingDefinition)
        {
            var classMap = new ClassMap<ExampleClass>();
            var map = classMap.CompositeId()
                .KeyProperty(x => x.Id)
                .KeyReference(x => x.Parent, null, mappingDefinition);

            mapping = classMap;
        }

        private void VerifyModel(Action<KeyManyToOneMapping> modelVerification)
        {
            var instructions = new PersistenceInstructions();
            instructions.AddSource(new StubProviderSource(mapping));
            instructions.UseConventions(conventions);

            var generatedModels = instructions.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) != null)
                .Classes.First()
                .Id;

            modelVerification(((CompositeIdMapping)modelInstance).KeyManyToOnes.First());
        }

        #endregion
    }
}
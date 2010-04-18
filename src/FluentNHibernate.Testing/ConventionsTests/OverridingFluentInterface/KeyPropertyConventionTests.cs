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
    public class KeyPropertyConventionTests
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

        #region Helpers

        private void Convention(Action<IKeyPropertyInstance> convention)
        {
            conventions.Add(new KeyPropertyConventionBuilder().Always(convention));
        }

        private void Mapping(Action<KeyPropertyPart> mappingDefinition)
        {
            var classMap = new ClassMap<ExampleClass>();
            var map = classMap.CompositeId()
                .KeyProperty(x => x.Id, mappingDefinition)
                .KeyReference(x => x.Parent);

            mapping = classMap;
        }

        private void VerifyModel(Action<KeyPropertyMapping> modelVerification)
        {
            var instructions = new PersistenceInstructions();
            instructions.AddSource(new StubProviderSource(mapping));
            instructions.UseConventions(conventions);

            var generatedModels = instructions.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == typeof(ExampleClass)) != null)
                .Classes.First()
                .Id;

            modelVerification(((CompositeIdMapping)modelInstance).KeyProperties.First());
        }

        #endregion
    }
}
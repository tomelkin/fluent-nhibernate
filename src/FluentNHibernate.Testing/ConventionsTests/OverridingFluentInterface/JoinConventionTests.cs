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
using FluentNHibernate.Testing.FluentInterfaceTests;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.OverridingFluentInterface
{
    [TestFixture]
    public class JoinConventionTests
    {
        private IProvider mapping;
        private Type mappingType;
        ConventionsCollection conventions;

        [SetUp]
        public void CreatePersistenceModel()
        {
            conventions = new ConventionsCollection();
        }

        [Test]
        public void TableShouldntBeOverwritten()
        {
            Mapping(x => x.Table("table"));

            Convention(x => x.Table("xxx"));

            VerifyModel(x => x.TableName.ShouldEqual("table"));
        }

        [Test]
        public void SchemaShouldntBeOverwritten()
        {
            Mapping(x => x.Schema("dbo"));

            Convention(x => x.Schema("xxx"));

            VerifyModel(x => x.Schema.ShouldEqual("dbo"));
        }

        [Test]
        public void SubselectShouldntBeOverwritten()
        {
            Mapping(x => x.Subselect("select"));

            Convention(x => x.Subselect("xxx"));

            VerifyModel(x => x.Subselect.ShouldEqual("select"));
        }

        [Test]
        public void FetchShouldntBeOverwritten()
        {
            Mapping(x => x.Fetch.Join());

            Convention(x => x.Fetch.Select());

            VerifyModel(x => x.Fetch.ShouldEqual("join"));
        }

        [Test]
        public void InverseShouldntBeOverwritten()
        {
            Mapping(x => x.Inverse());

            Convention(x => x.Not.Inverse());

            VerifyModel(x => x.Inverse.ShouldBeTrue());
        }

        [Test]
        public void OptionalShouldntBeOverwritten()
        {
            Mapping(x => x.Optional());

            Convention(x => x.Not.Optional());

            VerifyModel(x => x.Optional.ShouldBeTrue());
        }

        #region Helpers

        private void Convention(Action<IJoinInstance> convention)
        {
            conventions.Add(new JoinConventionBuilder().Always(convention));
        }

        private void Mapping(Action<JoinPart<ExampleClass>> mappingDefinition)
        {
            var classMap = new ClassMap<ExampleClass>();
            classMap.Id(x => x.Id);

            classMap.Join("table", mappingDefinition);

            mapping = classMap;
            mappingType = typeof(ExampleClass);
        }

        private void VerifyModel(Action<JoinMapping> modelVerification)
        {
            var instructions = new PersistenceInstructions();
            instructions.AddActions(mapping);
            instructions.UseConventions(conventions);

            var generatedModels = instructions.BuildMappings();
            var modelInstance = generatedModels
                .First(x => x.Classes.FirstOrDefault(c => c.Type == mappingType) != null)
                .Classes.First()
                .Joins.First();

            modelVerification(modelInstance);
        }

        #endregion
    }
}
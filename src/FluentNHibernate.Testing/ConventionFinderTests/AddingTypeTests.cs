using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionFinderTests
{
    [TestFixture]
    public class AddingTypeTests
    {
        private ConventionContainer container;

        [SetUp]
        public void CreateFinder()
        {
            container = new ConventionContainer(new ConventionsCollection());
        }

        [Test]
        public void AddingSingleShouldntThrowIfHasParameterlessConstructor()
        {
            var ex = Catch.Exception(() => container.Add<ConventionWithParameterlessConstructor>());

            ex.ShouldBeNull();
        }

        [Test]
        public void AddingSingleShouldntThrowIfHasIConventionFinderConstructor()
        {
            var ex = Catch.Exception(() => container.Add<ConventionWithIConventionFinderConstructor>());

            ex.ShouldBeNull();
        }

        [Test]
        public void AddingSingleShouldThrowIfNoParameterlessConstructor()
        {
            var ex = Catch.Exception(() => container.Add<ConventionWithoutValidConstructor>());

            ex.ShouldBeOfType<MissingConstructorException>();
            ex.ShouldNotBeNull();
        }

        [Test]
        public void AddingSingleShouldThrowIfNoIConventionFinderConstructor()
        {
            var ex = Catch.Exception(() => container.Add<ConventionWithoutValidConstructor>());

            ex.ShouldBeOfType<MissingConstructorException>();
            ex.ShouldNotBeNull();
        }

        [Test]
        public void AddingAssemblyShouldntThrowIfNoIConventionFinderConstructor()
        {
            var ex = Catch.Exception(() => container.AddAssembly(typeof(ConventionWithoutValidConstructor).Assembly));

            ex.ShouldBeNull();
        }
    }

    public class ConventionWithParameterlessConstructor : IClassConvention
    {
        public ConventionWithParameterlessConstructor()
        { }

        public void Apply(IClassInstance instance) {}
    }

    public class ConventionWithIConventionFinderConstructor : IClassConvention
    {
        public ConventionWithIConventionFinderConstructor(IConventionFinder conventionFinder)
        { }

        public void Apply(IClassInstance instance) {}
    }

    public class ConventionWithoutValidConstructor : IClassConvention
    {
        public ConventionWithoutValidConstructor(int someParameter)
        { }

        public void Apply(IClassInstance instance) {}
    }
}
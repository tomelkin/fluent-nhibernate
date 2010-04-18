using FluentNHibernate.Conventions;
using NUnit.Framework;

namespace FluentNHibernate.Testing.ConventionsTests.ApplyingToModel
{
    public abstract class BaseConventionTestFixture
    {
        ConventionsCollection conventions;

        [SetUp]
        public void CreatePersistenceModel()
        {
            conventions = new ConventionsCollection();
        }

        protected void Convention(IConvention convention)
        {
            conventions.Add(convention);
        }
    }
}
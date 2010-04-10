using System.Text;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Specs.FluentInterface.Fixtures;
using Machine.Specifications;

namespace FluentNHibernate.Specs.FluentInterface.Model
{
    public class when_creating_the_model_from_subclass_map
    {
        Establish context = () =>
        {
            tester = new ProviderToModelMappingTester<SubclassMap<EntityWithPrivateProperties>, SubclassMapping>(
                () => new SubclassMapping(SubclassType.Subclass));
            tester.Ignore(x => x.SubclassType);
            tester.Ignore(x => x.Name);
            tester.Ignore(x => x.Type);
            tester.Ignore(x => x.Extends); // TODO: Support Extends
            tester.Ignore(x => x.Key);
            tester.Pair(x => x.Lazy, x => x.LazyLoad());
            tester.Pair(x => x.TableName, x => x.Table(null));
            tester.Pair(x => x.Proxy, x => x.Proxy<ProxyClass>());
            tester.Pair(x => x.Persister, x => x.Persister<PersisterClass>());
        };

        Because of = () =>
            isValid = tester.Validate();

        It should_set_all_the_model_properties_correctly = () =>
            isValid.ShouldBeTrue();

        static ProviderToModelMappingTester<SubclassMap<EntityWithPrivateProperties>, SubclassMapping> tester;
        static bool isValid;
    }
}
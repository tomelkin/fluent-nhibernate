using FluentNHibernate.Infrastructure;
using FluentNHibernate.MappingModel.ClassBased;
using Machine.Specifications;

namespace FluentNHibernate.Specs.Compiler
{
    public abstract class MappingCompilerSpec
    {
        Establish context = () =>
        {
            instructions = new PersistenceInstructions();
            compiler = new MockCompiler(instructions);
        };

        protected static MockCompiler compiler;
        protected static PersistenceInstructions instructions;
        protected static ClassMapping mapping;

        protected class MockCompiler : MappingCompiler
        {
            public MockCompiler(IPersistenceInstructions instructions)
                : base(instructions)
            {}

            public override ITopMapping ManualMap(ITopMapping mapping)
            {
                ManualMapCalled = true;
                return base.ManualMap(mapping);
            }

            public override ITopMapping AutoMap(ITopMapping mapping)
            {
                AutoMapCalled = true;
                return base.AutoMap(mapping);
            }

            public bool ManualMapCalled { get; set; }
            public bool AutoMapCalled { get; set; }
        }
    }
}
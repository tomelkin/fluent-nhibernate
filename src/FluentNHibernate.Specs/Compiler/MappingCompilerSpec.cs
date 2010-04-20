using System;
using System.Collections.Generic;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
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
            compiler = new MockCompiler(
                new AutomapperV2(new ConventionFinder(instructions.Conventions)),
                instructions);
        };

        protected static MockCompiler compiler;
        protected static PersistenceInstructions instructions;
        protected static ClassMapping mapping;

        protected class MockCompiler : MappingCompiler
        {
            public MockCompiler(IAutomapper automapper, IPersistenceInstructions instructions)
                : base(automapper, instructions)
            {}

            public override IEnumerable<ITopMapping> ManualMap(ManualAction action)
            {
                ManualMapCalled = true;
                return base.ManualMap(action);
            }

            public override IEnumerable<ITopMapping> AutoMap(AutomapAction action)
            {
                AutoMapCalled = true;
                return base.AutoMap(action);
            }

            public bool ManualMapCalled { get; set; }
            public bool AutoMapCalled { get; set; }
        }
    }
}
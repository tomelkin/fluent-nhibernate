using System;
using FluentNHibernate.Automapping;

namespace FluentNHibernate
{
    public class AutomappingBuilder
    {
        readonly AutomappingInstructions instructions;

        public AutomappingBuilder(AutomappingInstructions instructions)
        {
            this.instructions = instructions;
        }

        public AutomappingBuilder ThisAssembly()
        {
            throw new NotImplementedException();
        }

        public AutomappingBuilder UsingConfiguration<T>()
            where T : IAutomappingConfiguration, new()
        {
            return UsingConfiguration(new T());
        }

        public AutomappingBuilder UsingConfiguration(IAutomappingConfiguration cfg)
        {
            instructions.Configuration = cfg;
            return this;
        }
    }
}
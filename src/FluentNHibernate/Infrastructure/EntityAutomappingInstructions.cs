using System;
using System.Collections.Generic;
using FluentNHibernate.Automapping;

namespace FluentNHibernate.Infrastructure
{
    // TODO: Use this class to combine the main instructions with individual
    // entity instructions. Currently just delegates everything to the main instructions
    public class EntityAutomappingInstructions : IAutomappingInstructions
    {
        readonly IAutomappingInstructions mainInstructions;

        public EntityAutomappingInstructions(IAutomappingInstructions mainInstructions)
        {
            this.mainInstructions = mainInstructions;
        }

        public IAutomappingConfiguration Configuration
        {
            get { return mainInstructions.Configuration ?? new DefaultAutomappingConfiguration(); }
        }

        public void UseConfiguration(IAutomappingConfiguration cfg)
        {
            throw new NotImplementedException();
        }

        public void AddSource(ITypeSource source)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Type> GetTypesToMap()
        {
            throw new NotImplementedException();
        }
    }
}
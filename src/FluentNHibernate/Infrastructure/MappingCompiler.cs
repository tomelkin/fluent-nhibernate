using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Automapping;
using FluentNHibernate.Conventions;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;

namespace FluentNHibernate.Infrastructure
{
    public interface IMappingCompiler
    {
        IEnumerable<ITopMapping> Compile(IMappingAction action);
    }

    public class MappingCompiler : IMappingCompiler
    {
        readonly IAutomapper automapper;
        readonly IPersistenceInstructions instructions;

        public MappingCompiler(IAutomapper automapper, IPersistenceInstructions instructions)
        {
            this.automapper = automapper;
            this.instructions = instructions;
        }

        public IEnumerable<HibernateMapping> BuildMappings()
        {
            var actions = instructions.GetActions();
            var mappings = CompileActions(actions);

            instructions.Visitors
                .Each(x => x.Visit(mappings));

            return mappings;
        }

        IEnumerable<HibernateMapping> CompileActions(IEnumerable<IMappingAction> actions)
        {
            var mappings = actions.SelectMany(x => Compile(x));
            var hbm = new HibernateMapping();

            mappings.Each(x => x.AddTo(hbm));

            return new[] { hbm };
        }

        public virtual IEnumerable<ITopMapping> AutoMap(AutomapAction action)
        {
            var mainInstructions = instructions.AutomappingInstructions;
            var targets = action.GetMappingTargets(mainInstructions);

            return automapper.Map(targets);
        }

        public virtual IEnumerable<ITopMapping> ManualMap(ManualAction action)
        {
            return new[] { action.GetMapping() };
        }

        public IEnumerable<ITopMapping> Compile(IMappingAction action)
        {
            if (action is ManualAction)
                return ManualMap((ManualAction)action);
            if (action is AutomapAction)
                return AutoMap((AutomapAction)action);

            throw new InvalidOperationException(string.Format("Unrecognised action '{0}'", action.GetType().FullName));
        }
    }
}
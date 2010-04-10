using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FluentNHibernate.Infrastructure;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.Utils;
using Machine.Specifications;

namespace FluentNHibernate.Specs.FluentInterface.Model
{
    public class ProviderToModelMappingTester<TProvider, TModel>
        where TProvider : IProvider, new()
    {
        readonly Func<TModel> createModel;
        readonly List<string> ignored = new List<string>();
        readonly List<Paired> manual = new List<Paired>();

        public ProviderToModelMappingTester(Func<TModel> createModel)
        {
            this.createModel = createModel;
        }

        public void Ignore(Expression<Func<TModel, object>> exp)
        {
            ignored.Add(exp.ToMember().Name);
        }

        public void Pair(Expression<Func<TModel, object>> leftExp, Expression<Func<TProvider, object>> rightExp)
        {
            manual.Add(new Paired { Property = leftExp.ToMember(), Method = (MethodMember)rightExp.ToMember() });
        }

        public void Pair(Expression<Func<TModel, object>> leftExp, Expression<Action<TProvider>> rightExp)
        {
            manual.Add(new Paired { Property = leftExp.ToMember(), Method = (MethodMember)rightExp.ToMember() });
        }

        IEnumerable<Paired> GetPairs()
        {
            var modelProperties = typeof(TModel).GetProperties()
                .Select(x => x.ToMember())
                .Where(x => !x.PropertyType.Closes(typeof(IEnumerable<>)))
                .Where(x => !ignored.Contains(x.Name))
                .Where(x => !manual.Select(z => z.Property.Name).Contains(x.Name))
                .ToArray();
            var providerMethods = typeof(TProvider).GetMethods()
                .Select(x => x.ToMember());

            var pairs = from p in modelProperties
            join m in providerMethods on p.Name equals m.Name
            select new Paired { Property = p, Method = (MethodMember)m };
            pairs = pairs.Concat(manual);

            var unpaired = modelProperties.Except(pairs.Select(x => x.Property)); 

            if (unpaired.Any())
                unpaired.Each(x =>
                {
                    throw new InvalidOperationException("Unpaired properties found: " + x.Name); 
                });

            return pairs;
        }

        class Paired
        {
            public Member Property { get; set; }
            public MethodMember Method { get; set; }
        }

        public bool Validate()
        {
            var pairs = GetPairs();

            foreach (var pair in pairs)
            {
                var provider = new TProvider();

                if (pair.Method.HasParameters)
                {
                    var parameters = pair.Method.GetParameters();

                    if (parameters.Count() > 1)
                        throw new InvalidOperationException("Can't call methods with more than one parameter.");

                    var parameter = parameters.First();
                    var parameterValue = GetRandomValueFor(parameter);

                    pair.Method.Invoke(provider, parameterValue);

                    var model = GetModel(provider);
                    var newValue = pair.Property.GetValue(model);

                    if (!parameterValue.Equals(newValue))
                        throw new SpecificationException(string.Format("Should have new value for {0} after calling {1}({2}) but had {3}", pair.Property.Name, pair.Method.Name, pretty(parameterValue), pretty(newValue)));
                }
                else
                {
                    var defaultModel = createModel();
                    var defaultValue = pair.Property.GetValue(defaultModel);

                    pair.Method.Invoke(provider);

                    var model = GetModel(provider);
                    var newValue = pair.Property.GetValue(model);

                    if (newValue.Equals(defaultValue))
                        throw new SpecificationException(string.Format("Should have new value for {0} after calling {1} but had original value of {2}", pair.Property.Name, pair.Method.Name, pretty(defaultValue)));
                }
            }

            return true;
        }

        string pretty(object value)
        {
            if (value is string)
                return "\"" + value + "\"";
            return value.ToString();
        }

        ITopMapping GetModel(IProvider provider)
        {
            var action = provider.GetAction();
            var mapping = action.GetType().GetField("mapping", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(action); // TODO: Wow, this is nasty

            return (ITopMapping)mapping;
        }

        object GetRandomValueFor(Type parameter)
        {
            if (parameter == typeof(int))
                return 1985;
            if (parameter == typeof(string))
                return "Luke Skywalker";
            if (parameter == typeof(DateTime))
                return DateTime.Now;
            if (parameter == typeof(object))
                return "Darth Vader"; // not sure we should do this

            throw new InvalidOperationException("Can't get default value for " + parameter.Name);
        }
    }
}
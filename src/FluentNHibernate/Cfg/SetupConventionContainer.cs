using System;
using System.Collections.Generic;
using System.Reflection;
using FluentNHibernate.Conventions;

namespace FluentNHibernate.Cfg
{
    public class SetupConventionContainer<TReturn> : IConventionContainer
    {
        private readonly TReturn parent;
        private readonly IConventionContainer container;

        public SetupConventionContainer(TReturn parent, IConventionContainer container)
        {
            this.parent = parent;
            this.container = container;
        }

        public TReturn AddAssembly(Assembly assembly)
        {
            container.AddAssembly(assembly);
            return parent;
        }

        public TReturn AddFromAssemblyOf<T>()
        {
            container.AddFromAssemblyOf<T>();
            return parent;
        }

        void IConventionContainer.AddFromAssemblyOf<T>()
        {
            AddFromAssemblyOf<T>();
        }

        void IConventionContainer.AddAssembly(Assembly assembly)
        {
            AddAssembly(assembly);
        }

        public TReturn Add<T>() where T : IConvention
        {
            container.Add<T>();
            return parent;
        }

        void IConventionContainer.Add<T>()
        {
            Add<T>();
        }

        public void Add(Type type, object instance)
        {
            container.Add(type, instance);
        }

        public TReturn Add<T>(T instance) where T : IConvention
        {
            container.Add(instance);
            return parent;
        }

        void IConventionContainer.Add(Type type)
        {
            Add(type);
        }

        public TReturn Add(Type type)
        {
            container.Add(type);
            return parent;
        }

        void IConventionContainer.Add<T>(T instance)
        {
            Add(instance);
        }

        public TReturn Add(params IConvention[] instances)
        {
            foreach (var instance in instances)
            {
                container.Add(instance.GetType(), instance);
            }

            return parent;
        }

        public TReturn Setup(Action<IConventionContainer> setupAction)
        {
            setupAction(this);
            return parent;
        }
    }
}
using Autofac;
using Autofac.Features.AttributeFilters;
using System;
using System.Reflection;

namespace GeneralConfigSetter.Services
{
    public static class Container
    {
        private static ContainerBuilder _builder;
        private static IContainer _container;

        static Container()
        {
            _builder = new ContainerBuilder();
        }

        public static void RegisterSingleInstancesFromAssembly(Assembly assembly)
        {
            _builder.RegisterAssemblyTypes(assembly).SingleInstance().WithAttributeFiltering();
        }

        public static void RegisterType(Type type)
        {
            _builder.RegisterType(type).SingleInstance().WithAttributeFiltering();
        }

        public static void RegisterTypeWithInterface<Tinterface, Timplementation>()
        {
            _builder.RegisterType<Timplementation>().As<Tinterface>().SingleInstance();
        }

        public static void RegisterKeyedType<T>(Type implementingType, string name)
        {
            _builder.RegisterType(implementingType).Keyed<T>(name).SingleInstance().WithAttributeFiltering();
        }

        public static void Build()
        {
            _container = _builder.Build();
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public static T ResolveKeyed<T>(string name)
        {
            return _container.ResolveKeyed<T>(name);
        }

        public static void Shutdown()
        {
            _container.Dispose();
        }
    }
}

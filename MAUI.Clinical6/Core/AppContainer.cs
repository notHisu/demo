using System;
using Autofac;
using IContainer = Autofac.IContainer;

namespace Xamarin.Forms.Clinical6.Core
{
    /// <summary>
    /// Exposes a DI container
    /// </summary>
    public interface IAppContainer
    {
        T Resolve<T>();

        object Resolve(Type type);
    }


    /// <summary>
    /// A service locator for view models and other dependencies
    /// </summary>
    /// <remarks>
    /// It's generally better to use constructor/property injection and reference this as little as possible
    /// </remarks>
    public class AppContainer : IAppContainer
    {
        private static AppContainer _instance;
        public static AppContainer Current => _instance ?? (_instance = new AppContainer());

        private IContainer _container;

        protected AppContainer()
        {
        }

        /// <summary>
        /// Initialize all dependencies based on the provided custom logic
        /// </summary>
        public void Initialize(Action<ContainerBuilder> configure)
        {
            var builder = new ContainerBuilder();

            configure(builder);

            _container = builder.Build();
        }

        public T Resolve<T>()
        {
            if (_container == null)
                throw new InvalidOperationException("App Container is not initialized");
            return _container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            if (_container == null)
                throw new InvalidOperationException("App Container is not initialized");
            return _container.Resolve(type);
        }
    }
}
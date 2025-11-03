using System;
using Autofac;

namespace Inventiva
{
    internal static class DependencyConfig
    {
        /// <summary>
        /// Wires up common dependencies
        /// </summary>
        public static void Configure(ContainerBuilder builder)
        {
            Xamarin.Forms.Clinical6.UI.DependencyConfig.Configure(builder);
            // ADD CUSTOM PAGES HERE
            // Xamarin.Forms.Clinical6.UI.DependencyConfig.RegisterRenderViewModel<MainViewModel, HomePage>(builder);
            RegisterServices(builder);
        }


        private static void RegisterServices(ContainerBuilder builder)
        {

        }
    }
}

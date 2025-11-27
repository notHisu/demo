using System;
using Xamarin.Forms.Clinical6.Core.Helpers;

namespace Xamarin.Forms.Clinical6.UI.MarkupExtensions
{
    /// <summary>
    /// XAML Markup Extension for retrieving localized resource strings
    /// </summary>
    [ContentProperty(nameof(ResourceName))]
    public class LocalizedExtension : IMarkupExtension<string>
    {
        public string ResourceName { get; set; }

        public string ProvideValue(IServiceProvider serviceProvider)
        {
            return ResourceName.Localized();
        }

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
        {
            return (this as IMarkupExtension<string>).ProvideValue(serviceProvider);
        }
    }
}
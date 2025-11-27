using System;
using System.Windows.Input;

namespace Xamarin.Forms.Clinical6.UI.Views.Card
{
    public interface IMaterialCard
    {
        string Id { set; get; }
        string Type { set; get; }
        int Priority { set; get; }
        string AdditionalInfo { get; set; }
        string Body { get; set; }
        string ImageSource { get; set; }
        string Label { get; set; }
        string Title { get; set; }
        bool IsVisible { get; set; }
        object ButtonReference { get; set; }
        bool ButtonIsVisible { get; set; }
        string ButtonText { get; set; }
    }
}

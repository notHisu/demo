using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace Xamarin.Forms.Clinical6.UI.Controls
{
    public partial class MaterialCardView : ContentView
    {
        //public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create<MaterialCardView, Color>(c => c.BackgroundColor, Color.Transparent);
        //public static readonly BindableProperty BodyProperty = BindableProperty.Create<MaterialCardView, string>(c => c.Body, string.Empty);
        //public static readonly BindableProperty LabelProperty = BindableProperty.Create<MaterialCardView, string>(c => c.Label, string.Empty);
        //public static readonly BindableProperty TitleProperty = BindableProperty.Create<MaterialCardView, string>(c => c.Title, string.Empty);

        public static readonly BindableProperty AdditionalInfoProperty  = BindableProperty.Create(nameof(AdditionalInfo),   typeof(string), typeof(MaterialCardView), string.Empty, propertyChanged: OnDataChanged);
        public static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor),  typeof(Color),  typeof(MaterialCardView), Colors.Transparent, propertyChanged: OnDataChanged);
        public static readonly BindableProperty BodyProperty            = BindableProperty.Create(nameof(Body),             typeof(string), typeof(MaterialCardView), string.Empty, propertyChanged: OnDataChanged);

        public static readonly BindableProperty ButtonIsVisibleProperty = BindableProperty.Create(nameof(ButtonIsVisible),  typeof(bool),   typeof(MaterialCardView), false, propertyChanged: OnDataChanged);
        public static readonly BindableProperty ButtonTextProperty      = BindableProperty.Create(nameof(ButtonText),       typeof(string), typeof(MaterialCardView), string.Empty, propertyChanged: OnDataChanged);
        public static readonly BindableProperty ButtonBindingContextProperty = BindableProperty.Create(nameof(ButtonBindingContext), typeof(object), typeof(MaterialCardView), null, propertyChanged: OnDataChanged);
        public static readonly BindableProperty ButtonCommandProperty = BindableProperty.Create(nameof(ButtonCommand), typeof(ICommand), typeof(MaterialCardView), null, propertyChanged: OnDataChanged);
        public static readonly BindableProperty ButtonCommandParameterProperty = BindableProperty.Create(nameof(ButtonCommandParameter), typeof(object), typeof(MaterialCardView), null, propertyChanged: OnDataChanged);


        public static readonly BindableProperty ImageSourceProperty     = BindableProperty.Create(nameof(ImageSource),      typeof(string), typeof(MaterialCardView), string.Empty, propertyChanged: OnDataChanged);
        public static readonly BindableProperty LabelProperty           = BindableProperty.Create(nameof(Label),            typeof(string), typeof(MaterialCardView), string.Empty, propertyChanged: OnDataChanged);
        public static readonly BindableProperty TitleProperty           = BindableProperty.Create(nameof(Title),            typeof(string), typeof(MaterialCardView), string.Empty, propertyChanged: OnDataChanged);

        public string AdditionalInfo
        {
            get { return (string)GetValue(AdditionalInfoProperty); }
            set { SetValue(AdditionalInfoProperty, value); }
        }

        public string Body
        {
            get { return (string)GetValue(BodyProperty); }
            set { SetValue(BodyProperty, value); }
        }

        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public bool ButtonIsVisible
        {
            get { return (bool)GetValue(ButtonIsVisibleProperty); }
            set { SetValue(ButtonIsVisibleProperty, value); }
        }

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        public object ButtonBindingContext
        {
            get { return (object)GetValue(ButtonBindingContextProperty); }
            set { SetValue(ButtonBindingContextProperty, value); }
        }

        public ICommand ButtonCommand
        {
            get { return (ICommand)GetValue(ButtonCommandProperty); }
            set { SetValue(ButtonCommandProperty, value); }
        }

        public object ButtonCommandParameter
        {
            get { return (object)GetValue(ButtonCommandParameterProperty); }
            set { SetValue(ButtonCommandParameterProperty, value); }
        }

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public MaterialCardView()
        {
            InitializeComponent();
        }

        private static void OnDataChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is MaterialCardView cView)
            {
                // Setting the text of this element manually instead of using bindings
                cView.CardAdditionalInfo.Text = cView.AdditionalInfo;
                cView.CardLabel.Text = cView.Label;
                cView.CardBody.Text = cView.Body;
                cView.CardTitle.Text = cView.Title;
                cView.CardFrame.BackgroundColor = cView.BackgroundColor;
                cView.CardImage.Source = cView.ImageSource;

                cView.CardButton.IsVisible = cView.ButtonIsVisible;
                cView.CardButton.Text = cView.ButtonText;
                cView.CardButton.BindingContext = cView.ButtonBindingContext;
                cView.CardButton.Command = cView.ButtonCommand;
                cView.CardButton.CommandParameter = cView.ButtonCommandParameter;
            }
        }

    }
}

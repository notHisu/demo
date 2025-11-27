using System;
using System.Windows.Input;

namespace Xamarin.Forms.Clinical6.UI.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomNavigationBar : Grid
    {
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomNavigationBar), defaultValue: string.Empty, propertyChanged: OnTitleChanged);

        private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var navigationBar = (CustomNavigationBar)bindable;

            if (newValue == null)
                return;

            if (newValue.ToString().Length > 23)
            {
                navigationBar._titleLabel.HorizontalTextAlignment = TextAlignment.Start;
                navigationBar._titleLabel.Margin = new Thickness(95, 0, 1, 0);
            }

            navigationBar._titleLabel.Text = newValue.ToString();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly BindableProperty TitleIsVisibleProperty = BindableProperty.Create(nameof(TitleIsVisible), typeof(bool), typeof(CustomNavigationBar), defaultValue: true, propertyChanged: OnTitleVisibleChanged);

        private static void OnTitleVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var navigationBar = (CustomNavigationBar)bindable;
            navigationBar._titleLabel.IsVisible = (bool)newValue;
        }

        public bool TitleIsVisible
        {
            get { return (bool)GetValue(TitleIsVisibleProperty); }
            set { SetValue(TitleIsVisibleProperty, value); }
        }

        public static readonly BindableProperty ButtonTextProperty = BindableProperty.Create(nameof(ButtonText), typeof(string), typeof(CustomNavigationBar), defaultValue: string.Empty, propertyChanged: OnButtonTextChanged);

        private static void OnButtonTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var navigationBar = (CustomNavigationBar)bindable;

            if (newValue == null)
                return;

            navigationBar._button.AutomationId =
                navigationBar._button.Text = newValue.ToString();
        }

        public string ButtonText
        {
            get { return (string)GetValue(ButtonTextProperty); }
            set { SetValue(ButtonTextProperty, value); }
        }

        public static readonly BindableProperty ButtonIsVisibleProperty = BindableProperty.Create(nameof(ButtonIsVisible), typeof(bool), typeof(CustomNavigationBar), defaultValue: true, propertyChanged: OnButtonVisibleChanged);

        private static void OnButtonVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var navigationBar = (CustomNavigationBar)bindable;
            navigationBar._button.IsVisible = (bool)newValue;
        }

        public bool ButtonIsVisible
        {
            get { return (bool)GetValue(ButtonIsVisibleProperty); }
            set { SetValue(ButtonIsVisibleProperty, value); }
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CustomNavigationBar), defaultValue: null, propertyChanged: OnCommandChanged);

        private static void OnCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var navigationBar = (CustomNavigationBar)bindable;
            navigationBar._button.Command = newValue as ICommand;
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public event EventHandler Clicked;

        public CustomNavigationBar()
        {
            InitializeComponent();

            _button.Clicked += OnButtonClicked;
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            Clicked?.Invoke(sender, e);
        }
    }
}
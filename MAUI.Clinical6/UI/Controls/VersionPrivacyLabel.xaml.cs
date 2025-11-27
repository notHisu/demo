using System.Windows.Input;

namespace Xamarin.Forms.Clinical6.UI.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VersionPrivacyLabel : Label
    {
        public static readonly BindableProperty VersionTextProperty = BindableProperty.Create(nameof(VersionText), typeof(string), typeof(VersionPrivacyLabel), defaultValue: string.Empty, propertyChanged: OnVersionTextChanged);

        private static void OnVersionTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var versionPrivacyLabel = (VersionPrivacyLabel)bindable;

            if (newValue == null)
                return;

            versionPrivacyLabel._versionSpan.Text = newValue.ToString();
        }

        public string VersionText
        {
            get { return (string)GetValue(VersionTextProperty); }
            set { SetValue(VersionTextProperty, value); }
        }

        public static readonly BindableProperty VersionTextColorProperty = BindableProperty.Create(nameof(VersionTextColor), typeof(Color), typeof(VersionPrivacyLabel), defaultValue: Colors.Gray, propertyChanged: OnVersionTextColorChanged);

        private static void OnVersionTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var versionPrivacyLabel = (VersionPrivacyLabel)bindable;

            var color = (Color)newValue;
            versionPrivacyLabel._versionSpan.TextColor =
                versionPrivacyLabel._pipeSpan.TextColor = color;
        }

        public Color VersionTextColor
        {
            get { return (Color)GetValue(VersionTextColorProperty); }
            set { SetValue(VersionTextColorProperty, value); }
        }

        public static readonly BindableProperty PrivacyTextProperty = BindableProperty.Create(nameof(PrivacyText), typeof(string), typeof(VersionPrivacyLabel), defaultValue: string.Empty, propertyChanged: OnPrivacyTextChanged);

        private static void OnPrivacyTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var versionPrivacyLabel = (VersionPrivacyLabel)bindable;

            if (newValue == null)
                return;

            versionPrivacyLabel._privacySpan.Text = newValue.ToString();
        }

        public string PrivacyText
        {
            get { return (string)GetValue(PrivacyTextProperty); }
            set { SetValue(PrivacyTextProperty, value); }
        }

        public static readonly BindableProperty PrivacyTextColorProperty = BindableProperty.Create(nameof(PrivacyTextColor), typeof(Color), typeof(VersionPrivacyLabel), defaultValue: Colors.Black, propertyChanged: OnPrivacyTextColorChanged);

        private static void OnPrivacyTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var versionPrivacyLabel = (VersionPrivacyLabel)bindable;

            var color = (Color)newValue;
            versionPrivacyLabel._privacySpan.TextColor = color;
        }

        public Color PrivacyTextColor
        {
            get { return (Color)GetValue(PrivacyTextColorProperty); }
            set { SetValue(PrivacyTextColorProperty, value); }
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(VersionPrivacyLabel), defaultValue: null, propertyChanged: OnCommandChanged);

        private static void OnCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var versionPrivacyLabel = (VersionPrivacyLabel)bindable;
            versionPrivacyLabel._tapGestureRecognizer.Command = newValue as ICommand;
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public VersionPrivacyLabel()
        {
            InitializeComponent();
        }
    }
}
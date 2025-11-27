using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Xamarin.Forms.Clinical6.UI.Controls
{
    public class MaterialTextbox : View
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string),
                                                                                                typeof(MaterialTextbox),
                                                                                                defaultBindingMode: BindingMode.TwoWay, defaultValue: string.Empty);
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(nameof(MaxLength), typeof(int),
                                                                                               typeof(MaterialTextbox),
                                                                                               defaultBindingMode: BindingMode.TwoWay, defaultValue: 0);
        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        public static readonly BindableProperty PlaceHolderTextProperty = BindableProperty.Create(nameof(PlaceHolderText), typeof(string),
                                                                                              typeof(MaterialTextbox),
                                                                                              defaultBindingMode: BindingMode.TwoWay, defaultValue: string.Empty);
        public string PlaceHolderText
        {
            get { return (string)GetValue(PlaceHolderTextProperty); }
            set { SetValue(PlaceHolderTextProperty, value); }
        }

        public static readonly BindableProperty MatBorderColorProperty = BindableProperty.Create(nameof(MatBorderColor), typeof(string),
                                                                                            typeof(MaterialTextbox),
                                                                                            defaultBindingMode: BindingMode.TwoWay, defaultValue: "#a3a3a3");
        public string MatBorderColor
        {
            get { return (string)GetValue(MatBorderColorProperty); }
            set { SetValue(MatBorderColorProperty, value); }
        }

        public static readonly BindableProperty MatBorderColorErrorProperty = BindableProperty.Create(nameof(MatBorderErrorColor), typeof(string),
                                                                                           typeof(MaterialTextbox),
                                                                                           defaultBindingMode: BindingMode.TwoWay, defaultValue: string.Empty);
        public string MatBorderErrorColor
        {
            get { return (string)GetValue(MatBorderColorErrorProperty); }
            set { SetValue(MatBorderColorErrorProperty, value); }
        }


        public static readonly BindableProperty DroidTextBoxHeightProperty = BindableProperty.Create(nameof(DroidTextBoxHeight), typeof(int), typeof(MaterialTextbox),
                                                                                                     defaultBindingMode: BindingMode.TwoWay,
                                                                                                     defaultValue: 50);
        public int DroidTextBoxHeight
        {
            get { return (int)GetValue(DroidTextBoxHeightProperty); }
            set { SetValue(DroidTextBoxHeightProperty, value); }
        }

        public static readonly BindableProperty IOSTextBoxWidthProperty = BindableProperty.Create(nameof(IOSTextBoxWidth), typeof(int), typeof(MaterialTextbox),
                                                                                                   defaultBindingMode: BindingMode.TwoWay,
                                                                                                   defaultValue: 200);
        public int IOSTextBoxWidth
        {
            get { return (int)GetValue(IOSTextBoxWidthProperty); }
            set { SetValue(IOSTextBoxWidthProperty, value); }
        }

        public static readonly BindableProperty IOSTextBoxHeightProperty = BindableProperty.Create(nameof(IOSTextBoxHeight), typeof(int), typeof(MaterialTextbox),
                                                                                                   defaultBindingMode: BindingMode.TwoWay,
                                                                                                   defaultValue: 50);
        public int IOSTextBoxHeight
        {
            get { return (int)GetValue(IOSTextBoxHeightProperty); }
            set { SetValue(IOSTextBoxHeightProperty, value); }
        }


        public static readonly BindableProperty KeyboardTypeProperty = BindableProperty.Create(nameof(KeyboardType), typeof(Microsoft.Maui.Keyboard), typeof(MaterialTextbox),
                                                                                                         defaultBindingMode: BindingMode.TwoWay,
                                                                                                         defaultValue: Microsoft.Maui.Keyboard.Default);
        public Microsoft.Maui.Keyboard KeyboardType
        {
            get { return (Microsoft.Maui.Keyboard)GetValue(KeyboardTypeProperty); }
            set { SetValue(KeyboardTypeProperty, value); }
        }


        public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(nameof(ReturnKeyType), typeof(ReturnType), typeof(MaterialTextbox),
                                                                                                        defaultBindingMode: BindingMode.TwoWay,
                                                                                                        defaultValue: ReturnType.Default);
        public ReturnType ReturnKeyType
        {
            get { return (ReturnType)GetValue(ReturnTypeProperty); }
            set { SetValue(ReturnTypeProperty, value); }

        }

        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(MaterialTextbox),
                                                                                                       defaultBindingMode: BindingMode.TwoWay,
                                                                                                       defaultValue: false);
        public bool IsPassword
        {
            get { return (bool)GetValue(IsPasswordProperty); }
            set { SetValue(IsPasswordProperty, value); }

        }

        public static readonly BindableProperty LostFocusAndBlankProperty = BindableProperty.Create(nameof(LostFocusAndBlank), typeof(bool), typeof(MaterialTextbox),
                                                                                                   defaultBindingMode: BindingMode.TwoWay,
                                                                                                   defaultValue: false);
        public bool LostFocusAndBlank
        {
            get { return (bool)GetValue(LostFocusAndBlankProperty); }
            set { SetValue(LostFocusAndBlankProperty, value); }
        }

        public static readonly BindableProperty LostFocusAndInvalidEmailProperty = BindableProperty.Create(nameof(LostFocusAndInvalidEmail), typeof(bool), typeof(MaterialTextbox),
                                                                                                   defaultBindingMode: BindingMode.TwoWay,
                                                                                                   defaultValue: false);
        public bool LostFocusAndInvalidEmail
        {
            get { return (bool)GetValue(LostFocusAndInvalidEmailProperty); }
            set { SetValue(LostFocusAndInvalidEmailProperty, value); }
        }

        public static readonly BindableProperty LostFocusAndInvalidStringProperty = BindableProperty.Create(nameof(LostFocusAndInvalidString), typeof(bool), typeof(MaterialTextbox),
                                                                                                   defaultBindingMode: BindingMode.TwoWay,
                                                                                                   defaultValue: false);
        public bool LostFocusAndInvalidString
        {
            get { return (bool)GetValue(LostFocusAndInvalidStringProperty); }
            set { SetValue(LostFocusAndInvalidStringProperty, value); }
        }

        public static readonly BindableProperty LostFocusAndInvalidPasswordProperty = BindableProperty.Create(nameof(LostFocusAndInvalidPassword), typeof(bool), typeof(MaterialTextbox),
                                                                                                   defaultBindingMode: BindingMode.TwoWay,
                                                                                                   defaultValue: false);
        public bool LostFocusAndInvalidPassword
        {
            get { return (bool)GetValue(LostFocusAndInvalidPasswordProperty); }
            set { SetValue(LostFocusAndInvalidPasswordProperty, value); }
        }

        public static readonly BindableProperty RightIconProperty = BindableProperty.Create(nameof(RightIcon), typeof(string),
                                                                                            typeof(MaterialTextbox),
                                                                                            defaultBindingMode: BindingMode.TwoWay, defaultValue: string.Empty);
        public string RightIcon
        {
            get { return (string)GetValue(RightIconProperty); }
            set { SetValue(RightIconProperty, value); }
        }

        /// <summary>
        /// The tap press ICommand property.
        /// </summary>
        public static readonly BindableProperty TapPressCommandProperty = BindableProperty.Create(nameof(TapPressCommand), typeof(ICommand), typeof(MaterialTextbox));

        /// <summary>
        /// Gets or sets the tap press command.
        /// </summary>
        /// <value>The tap press command.</value>
        public ICommand TapPressCommand
        {
            get
            {
                return (ICommand)GetValue(TapPressCommandProperty);
            }
            set
            {
                SetValue(TapPressCommandProperty, value);
            }
        }

        // TODO: Move this to shared app area.
        public bool VerifyEmail(string email)
        {
            if (email == null)
                return false;

            var lowerEmail = email.ToLower();
            Regex regex = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");
            Match match = regex.Match(lowerEmail);

            return match.Success;
        }

        /// <summary>
        /// Verify String
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool VerifyString(string value)
        {
            return !(value == null || value == "");
        }

        // TODO: Move this to shared app area.
        public bool VerifyPassword(string password)
        {
            return !(password == null || password == "");
        }

        public void FocusChanged(bool hasFocus, string textBoxText)
        {
            if (!hasFocus)
            {
                LostFocusAndBlank = textBoxText == null || textBoxText == "";
                LostFocusAndInvalidEmail = VerifyEmail(textBoxText) == false;
                LostFocusAndInvalidString = VerifyString(textBoxText) == false;
                LostFocusAndInvalidPassword = VerifyPassword(textBoxText) == false;
            }
            else
            {
                LostFocusAndBlank = false;
                LostFocusAndInvalidEmail = false;
                LostFocusAndInvalidPassword = false;
                LostFocusAndInvalidString = false;
            }
        }

        protected override void ChangeVisualState()
        {
            base.ChangeVisualState();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
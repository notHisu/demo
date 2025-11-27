using Android.Animation;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.Core.Content;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Handlers;
using System;
using Xamarin.Forms.Clinical6.UI.Controls;
using static Android.App.ActionBar;

namespace Xamarin.Forms.Clinical6.Handlers
{
    public class MaterialTextboxHandler : ViewHandler<MaterialTextbox, RelativeLayout>
    {
        private FormsTextView? _placeHolderLabel;
        private FormsEditText? _editText;
        private ImageView? _roundedDrawableImageView;
        private const int PADDINGFORBASEVIEW = 10;

        public static IPropertyMapper<MaterialTextbox, MaterialTextboxHandler> PropertyMapper =
            new PropertyMapper<MaterialTextbox, MaterialTextboxHandler>(ViewHandler.ViewMapper)
            {
                [nameof(MaterialTextbox.Text)] = MapText,
                [nameof(MaterialTextbox.PlaceHolderText)] = MapPlaceholder,
                [nameof(MaterialTextbox.IsPassword)] = MapIsPassword,
                [nameof(MaterialTextbox.MaxLength)] = MapMaxLength,
            };

        public MaterialTextboxHandler() : base(PropertyMapper) { }

        protected override RelativeLayout CreatePlatformView()
        {
            var relativeLayout = new RelativeLayout(Context);

            var textbox = VirtualView;
            if (textbox == null)
                return relativeLayout;

            // --- create EditText
            _editText = new FormsEditText(Context)
            {
                Text = textbox.Text,
                ImeOptions = GetKeyboardReturnKeyType(textbox.ReturnKeyType),
            };
            _editText.SetRawInputType(GetKeyboardType(textbox.KeyboardType));
            _editText.LayoutParameters = new RelativeLayout.LayoutParams(
                LayoutParams.MatchParent,
                LayoutParams.WrapContent
            );
            _editText.TranslationY = 16;
            _editText.SetPadding(70, 40, 100, 0);
            _editText.Background = null;
            _editText.SetMaxLines(1);

            if (textbox.IsPassword)
            {
                _editText.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
            }

            if (textbox.MaxLength > 0)
            {
                _editText.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(textbox.MaxLength) });
            }

            _editText.FocusChange += (s, e) =>
            {
                textbox.FocusChanged(e.HasFocus, textbox.Text);
                if (!e.HasFocus)
                {
                    if (!string.IsNullOrEmpty(textbox.MatBorderErrorColor))
                        SetTextboxActiveErrorState();
                }
                else
                {
                    SetTextboxActiveState();
                }
            };

            _editText.TextChanged += (s, e) =>
            {
                textbox.Text = e.Text.ToString();
            };

            // --- border background
            var roundedDrawableContainer = new LinearLayout(Context)
            {
                TranslationY = _editText.TranslationY
            };
            roundedDrawableContainer.LayoutParameters = new RelativeLayout.LayoutParams(
                LayoutParams.MatchParent,
                (int)ToDP(textbox.DroidTextBoxHeight)
            );

            _roundedDrawableImageView = new ImageView(Context);
            _roundedDrawableImageView.LayoutParameters = new RelativeLayout.LayoutParams(
                LayoutParams.MatchParent,
                LayoutParams.MatchParent
            );
            _roundedDrawableImageView.SetImageDrawable(
                ContextCompat.GetDrawable(Context, MAUI.Clinical6.Resource.Drawable.zzz_edit_text_border)
            );

            roundedDrawableContainer.AddView(_roundedDrawableImageView);

            // --- placeholder
            _placeHolderLabel = new FormsTextView(Context)
            {
                Text = textbox.PlaceHolderText,
                TranslationY = ToDP(20f)
            };
            _placeHolderLabel.SetTextSize(ComplexUnitType.Sp, 16);
            _placeHolderLabel.TranslationX = 90;
            _placeHolderLabel.SetTextColor(global::Android.Graphics.Color.Gray);
            _placeHolderLabel.SetBackgroundColor(global::Android.Graphics.Color.White);

            // --- add all
            relativeLayout.AddView(roundedDrawableContainer);
            relativeLayout.AddView(_editText);
            relativeLayout.AddView(_placeHolderLabel);

            textbox.HeightRequest = textbox.DroidTextBoxHeight + PADDINGFORBASEVIEW;

            // start with animation if text is preset
            if (!string.IsNullOrWhiteSpace(_editText.Text))
            {
                PerformTranslationYAnimation();
            }

            return relativeLayout;
        }

        #region Property Mappers
        private static void MapText(MaterialTextboxHandler handler, MaterialTextbox textbox)
        {
            if (handler._editText == null) return;

            if (handler._editText.Text != textbox.Text)
                handler._editText.Text = textbox.Text ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(textbox.Text))
                handler.PerformTranslationYAnimation();
            else
                handler.ReverseTranslationYAnimation();
        }

        private static void MapPlaceholder(MaterialTextboxHandler handler, MaterialTextbox textbox)
        {
            if (handler._placeHolderLabel == null) return;
            handler._placeHolderLabel.Text = textbox.PlaceHolderText;
        }

        private static void MapIsPassword(MaterialTextboxHandler handler, MaterialTextbox textbox)
        {
            if (handler._editText == null) return;
            handler._editText.InputType = textbox.IsPassword
                ? InputTypes.ClassText | InputTypes.TextVariationPassword
                : InputTypes.ClassText | InputTypes.TextVariationNormal;
        }

        private static void MapMaxLength(MaterialTextboxHandler handler, MaterialTextbox textbox)
        {
            if (handler._editText == null) return;
            if (textbox.MaxLength > 0)
                handler._editText.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(textbox.MaxLength) });
        }
        #endregion

        #region Animation + Styling
        private void PerformTranslationYAnimation()
        {
            if (_placeHolderLabel == null) return;

            var animation = ObjectAnimator.OfFloat(_placeHolderLabel, "TranslationY", -ToDP(3.5f));
            animation.SetDuration(200);
            animation.AnimationEnd += (s, e) =>
            {
                _placeHolderLabel.SetTextSize(ComplexUnitType.Sp, 12);
                _placeHolderLabel.SetTextColor(global::Android.Graphics.Color.ParseColor(VirtualView.MatBorderColor));
                UpdateRoundedDrawable(3);
            };
            animation.Start();
        }

        private void ReverseTranslationYAnimation()
        {
            if (_placeHolderLabel == null) return;

            var animation = ObjectAnimator.OfFloat(_placeHolderLabel, "TranslationY", ToDP(20f));
            animation.SetDuration(200);
            animation.AnimationEnd += (s, e) =>
            {
                _placeHolderLabel.SetTextSize(ComplexUnitType.Sp, 16);
                UpdateRoundedDrawable(1);
            };
            animation.Start();
        }

        private void UpdateRoundedDrawable(int strokeWidth)
        {
            if (_roundedDrawableImageView == null) return;

            try
            {
                var drawable = (GradientDrawable)ContextCompat.GetDrawable(Context, MAUI.Clinical6.Resource.Drawable.zzz_edit_text_border);
                if (!VirtualView.MatBorderColor.StartsWith("#"))
                {
                    VirtualView.MatBorderColor = "#" + VirtualView.MatBorderColor;
                }
                drawable?.SetStroke(strokeWidth, global::Android.Graphics.Color.ParseColor(VirtualView.MatBorderColor));
                _roundedDrawableImageView.SetImageDrawable(drawable);
            }
            catch { }
        }

        private void SetTextboxActiveState()
        {
            if (_editText == null || _placeHolderLabel == null) return;
            if (string.IsNullOrWhiteSpace(_editText.Text))
                ReverseTranslationYAnimation();
        }

        private void SetTextboxActiveErrorState()
        {
            if (_editText == null || _placeHolderLabel == null) return;
            if (string.IsNullOrWhiteSpace(_editText.Text))
                UpdateErrorRoundedDrawable(2);
        }

        private void UpdateErrorRoundedDrawable(int strokeWidth)
        {
            if (_roundedDrawableImageView == null) return;

            if (string.IsNullOrEmpty(VirtualView.MatBorderErrorColor)) return;

            var drawable = (GradientDrawable)ContextCompat.GetDrawable(Context, MAUI.Clinical6.Resource.Drawable.zzz_edit_text_border);
            if (!VirtualView.MatBorderErrorColor.StartsWith("#"))
            {
                VirtualView.MatBorderErrorColor = "#" + VirtualView.MatBorderErrorColor;
            }
            drawable?.SetStroke(strokeWidth, global::Android.Graphics.Color.ParseColor(VirtualView.MatBorderErrorColor));
            _roundedDrawableImageView.SetImageDrawable(drawable);
        }
        #endregion

        #region Helpers
        private float ToDP(float size)
        {
            return TypedValue.ApplyDimension(ComplexUnitType.Dip, size, Context.Resources.DisplayMetrics);
        }

        private InputTypes GetKeyboardType(Keyboard keyboard)
        {
            if (keyboard == Keyboard.Telephone)
                return InputTypes.ClassPhone;

            if (keyboard == Keyboard.Email)
                // combine with ClassText for proper behavior
                return InputTypes.ClassText | InputTypes.TextVariationEmailAddress;

            if (keyboard == Keyboard.Numeric)
                return InputTypes.ClassNumber;

            if (keyboard == Keyboard.Text)
                return InputTypes.ClassText;

            if (keyboard == Keyboard.Url)
                // combine with ClassText as well
                return InputTypes.ClassText | InputTypes.TextVariationUri;

            return InputTypes.ClassText;
        }


        private ImeAction GetKeyboardReturnKeyType(ReturnType returnType)
        {
            try
            {
                return (ImeAction)Enum.Parse(typeof(ImeAction), returnType.ToString());
            }
            catch
            {
                return ImeAction.ImeNull;
            }
        }
        #endregion
    }
}


//using Android.Animation;
//using Android.Content;
//using Android.Content.PM;
//using Android.Graphics.Drawables;
//using Android.Text;
//using Android.Util;
//using Android.Views;
//using Android.Views.InputMethods;
//using Android.Widget;
//using AndroidX.Core.Content;
//using Microsoft.Maui.Controls.Compatibility.Platform.Android;
//using Microsoft.Maui.Controls.Platform;
//using System;
//using System.ComponentModel;
//using Xamarin.Forms;
//using Xamarin.Forms.Clinical6.Android.Renderers;
//using Xamarin.Forms.Clinical6.UI.Controls;
//using ExportRendererAttribute = Microsoft.Maui.Controls.Compatibility.ExportRendererAttribute;
//using View = Microsoft.Maui.Controls.View;

//[assembly: ExportRenderer(typeof(MaterialTextbox), typeof(MaterialTextboxRenderer))]
//namespace Xamarin.Forms.Clinical6.Android.Renderers
//{
//    /// <summary>
//    /// Material textbox renderer.
//    /// </summary>
//    public class MaterialTextboxRenderer : ViewRenderer
//    {
//        private FormsTextView placeHolderLabel;
//        private FormsEditText editText;
//        private MaterialTextbox materialTextbox;
//        private const int PADDINGFORBASEVIEW = 10;
//        private ImageView roundedDrawableImageView;

//        public MaterialTextboxRenderer(Context context) : base(context)
//        {
//        }

//        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
//        {
//            base.OnElementChanged(e);

//            // get base control from xamarin forms
//            materialTextbox = (MaterialTextbox)e.NewElement;

//            //create base container to hold textbox and label
//            var relativeLayout = new global::Android.Widget.RelativeLayout(Context);

//            //create edit text control and set background to custom drawable
//            CreateFormEditText(relativeLayout, e.NewElement);

//            CreateTextViewForPlaceHolder(relativeLayout);

//            SetNativeControl(relativeLayout);

//            //if MaterialTextbox have default text value 
//            //transition to active state
//            if (!string.IsNullOrWhiteSpace(editText.Text))
//            {
//                PerformTranslationYAnimation();
//            }
//        }

//        /// <summary>
//        /// Creates label for place holder.
//        /// </summary>
//        /// <param name="relativeLayout">Relative layout.</param>
//        private void CreateTextViewForPlaceHolder(global::Android.Widget.RelativeLayout relativeLayout)
//        {
//            placeHolderLabel = new FormsTextView(Context);
//            placeHolderLabel.Text = materialTextbox.PlaceHolderText;
//            placeHolderLabel.TranslationY = ToDP(20.0f);

//            placeHolderLabel.SetTextSize(ComplexUnitType.Sp, 16);
//            placeHolderLabel.TranslationX = 90;

//            placeHolderLabel.SetTextColor(global::Android.Graphics.Color.Gray);
//            placeHolderLabel.SetBackgroundColor(global::Android.Graphics.Color.White);

//            relativeLayout.AddView(placeHolderLabel, 1);
//        }

//        /// <summary>
//        /// Creates the form edit text.
//        /// </summary>
//        /// <param name="relativeLayout">Relative layout.</param>
//        /// <param name="xamarinView">Xamarin view.</param>
//        private void CreateFormEditText(global::Android.Widget.RelativeLayout relativeLayout, View xamarinView)
//        {
//            editText = new FormsEditText(Context);
//            editText.SetCursorVisible(true);
//            editText.Text = materialTextbox.Text;
//            editText.SetRawInputType(GetKeyboardType(materialTextbox.KeyboardType));
//            editText.ImeOptions = GetKeyboardReturnKeyType(materialTextbox.ReturnKeyType);
//            editText.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
//            editText.TranslationY = 16;
//            editText.SetPadding(70, 40, 100, 0);
//            editText.Background = null;
//            editText.SetMaxLines(1);

//            if (materialTextbox.KeyboardType == Keyboard.Email)
//            {
//                // Turn off auto corrections.
//                editText.InputType = InputTypes.TextFlagNoSuggestions;
//            }

//            if (this.materialTextbox.MaxLength > 0)
//            {
//                editText.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(materialTextbox.MaxLength) });
//            }

//            editText.InputType = materialTextbox.IsPassword ? InputTypes.ClassText | InputTypes.TextVariationPassword
//                                                            : InputTypes.ClassText | InputTypes.TextVariationNormal;

//            editText.FocusChange += (object sender, FocusChangeEventArgs e) =>
//            {

//                materialTextbox.FocusChanged(e.HasFocus, materialTextbox.Text);

//                if (!e.HasFocus)
//                {
//                    if (string.IsNullOrEmpty(materialTextbox.MatBorderErrorColor))
//                    {
//                        return;
//                    }
//                    SetTextboxActiveErrorState();
//                }
//                else
//                {
//                    SetTextboxActiveState();
//                }
//            };

//            editText.TextChanged += (object sender, global::Android.Text.TextChangedEventArgs e) =>
//            {
//                //update MaterialTextbox text property
//                materialTextbox.Text = e.Text.ToString();
//            };

//            var roundedDrawableContainer = new LinearLayout(Context);
//            roundedDrawableContainer.TranslationY = editText.TranslationY;
//            roundedDrawableContainer.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
//            roundedDrawableContainer.LayoutParameters.Height = (int)ToDP(materialTextbox.DroidTextBoxHeight);

//            roundedDrawableImageView = new ImageView(Context);
//            roundedDrawableImageView.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
//            roundedDrawableImageView.SetImageDrawable(ContextCompat.GetDrawable(Context, MAUI.Clinical6.Resource.Drawable.zzz_edit_text_border));
//            roundedDrawableContainer.AddView(roundedDrawableImageView);
//            roundedDrawableImageView.SetPadding(0, 0, 0, 0);

//            if (!string.IsNullOrEmpty(materialTextbox.RightIcon))
//            {
//                Context context = Controls.Instance;
//                PackageManager manager = context.PackageManager;

//                var _flagResourceId = MAUI.Clinical6.Resource.Drawable.bio_fingerprint_input_android;

//                editText.SetPaddingRelative(40, 40, 40, 0);
//                editText.SetCompoundDrawablesRelativeWithIntrinsicBounds(null, null, ContextCompat.GetDrawable(Context, _flagResourceId), null);
//                editText.SetOnTouchListener(new OnDrawableTouchListener(materialTextbox));
//            }

//            relativeLayout.AddView(roundedDrawableContainer);

//            relativeLayout.AddView(editText);

//            //set xamarin view height based on size of textbox
//            xamarinView.HeightRequest = materialTextbox.DroidTextBoxHeight + PADDINGFORBASEVIEW;
//        }

//        void EditText_EditorAction(object sender, TextView.EditorActionEventArgs e)
//        {
//            switch (e.ActionId)
//            {
//                case ImeAction.Done:
//                    break;
//                case ImeAction.Go:
//                    break;
//                case ImeAction.ImeMaskAction:
//                    break;
//                case ImeAction.Next:
//                    break;
//                case ImeAction.None:
//                    break;
//                case ImeAction.Previous:
//                    break;
//                case ImeAction.Search:
//                    break;
//                case ImeAction.Send:
//                    break;
//                default:
//                    break;
//            }
//        }

//        /// <summary>
//        /// Sets the state of the textbox active.
//        /// </summary>
//        private void SetTextboxActiveState()
//        {
//            if (editText == null || placeHolderLabel == null)
//            {
//                return;
//            }

//            if (string.IsNullOrWhiteSpace(editText.Text))
//            {
//                ReverseTranslationYAnimation();
//            }
//        }

//        private void SetTextboxActiveErrorState()
//        {
//            if (editText == null || placeHolderLabel == null)
//            {
//                return;
//            }

//            if (string.IsNullOrWhiteSpace(editText.Text))
//            {
//                UpdateErrorRoundedDrawable(2);
//            }
//        }

//        /// <summary>
//        /// Performs the translation Y animation.
//        /// </summary>
//        private void PerformTranslationYAnimation()
//        {
//            if (placeHolderLabel == null)
//            {
//                return;
//            }

//            var animation = ObjectAnimator.OfFloat(placeHolderLabel, "TranslationY", -ToDP(3.5f));
//            animation.AnimationEnd += PerformTranslationYAnimation_AnimationEnd;
//            animation.SetDuration(200);
//            animation.Start();
//        }

//        void PerformTranslationYAnimation_AnimationEnd(object sender, EventArgs e)
//        {
//            try
//            {
//                var animation = (ObjectAnimator)sender;

//                //remove event listener
//                animation.AnimationEnd -= PerformTranslationYAnimation_AnimationEnd;

//                //reset font size
//                placeHolderLabel.SetTextSize(ComplexUnitType.Sp, 12);
//                placeHolderLabel.SetTextColor(global::Android.Graphics.Color.ParseColor(materialTextbox.MatBorderColor));

//                //change stroke width
//                UpdateRoundedDrawable(3);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"PerformTranslationYAnimation_AnimationEnd Exception: {ex}");
//            }
//        }

//        /// <summary>
//        /// Updates rounded drawable stroke.
//        /// </summary>
//        /// <param name="strokeWidth">Stroke width.</param>
//        private void UpdateRoundedDrawable(int strokeWidth)
//        {
//            try
//            {
//                GradientDrawable drawable = (GradientDrawable)ContextCompat.GetDrawable(Context, MAUI.Clinical6.Resource.Drawable.zzz_edit_text_border);
//                if (!materialTextbox.MatBorderColor.Contains("#"))
//                {
//                    materialTextbox.MatBorderColor = materialTextbox.MatBorderColor.Insert(0, "#");
//                }
//                drawable.SetStroke(strokeWidth, global::Android.Graphics.Color.ParseColor(materialTextbox.MatBorderColor));
//                roundedDrawableImageView.SetImageDrawable(drawable);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"UpdateRoundedDrawable Exception: {ex}");
//            }
//        }

//        private void UpdateErrorRoundedDrawable(int strokeWidth)
//        {
//            try
//            {
//                if (string.IsNullOrEmpty(materialTextbox.MatBorderErrorColor))
//                {
//                    return;
//                }

//                GradientDrawable drawable = (GradientDrawable)ContextCompat.GetDrawable(Context, MAUI.Clinical6.Resource.Drawable.zzz_edit_text_border);
//                if (!materialTextbox.MatBorderErrorColor.Contains("#"))
//                {
//                    materialTextbox.MatBorderErrorColor = materialTextbox.MatBorderErrorColor.Insert(0, "#");
//                }
//                drawable.SetStroke(strokeWidth, global::Android.Graphics.Color.ParseColor(materialTextbox.MatBorderErrorColor));
//                roundedDrawableImageView.SetImageDrawable(drawable);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"UpdateRoundedDrawable Exception: {ex}");
//            }
//        }

//        /// <summary>
//        /// Reverses the translation YA nimation.
//        /// </summary>
//        private void ReverseTranslationYAnimation()
//        {
//            if (placeHolderLabel == null)
//            {
//                return;
//            }

//            var animation = ObjectAnimator.OfFloat(placeHolderLabel, "TranslationY", ToDP(20.0f));
//            animation.AnimationEnd += ReverseTranslationYAnimation_AnimationEnd;
//            animation.SetDuration(200);
//            animation.Start();
//        }

//        void ReverseTranslationYAnimation_AnimationEnd(object sender, EventArgs e)
//        {
//            var animation = (ObjectAnimator)sender;

//            //remove event listener
//            animation.AnimationEnd -= ReverseTranslationYAnimation_AnimationEnd;

//            //reset font size
//            placeHolderLabel.SetTextSize(ComplexUnitType.Sp, 16);

//            //change stroke width
//            UpdateRoundedDrawable(1);
//        }

//        /// <summary>
//        /// Tos the dp.
//        /// </summary>
//        /// <returns>The dp.</returns>
//        /// <param name="size">Size.</param>
//        private float ToDP(float size)
//        {
//            return TypedValue.ApplyDimension(ComplexUnitType.Dip, size, Resources.DisplayMetrics);
//        }

//        /// <summary>
//        /// Gets the type of the keyboard.
//        /// </summary>
//        /// <returns>The keyboard type.</returns>
//        /// <param name="keyboard">Keyboard.</param>
//        private InputTypes GetKeyboardType(Keyboard keyboard)
//        {
//            InputTypes keyboardType = InputTypes.ClassText;

//            try
//            {
//                var xamarinKeyboardName = keyboard.ToString().Substring(keyboard.ToString().LastIndexOf('.') + 1);
//                var keyboardStr = xamarinKeyboardName.Replace("Keyboard", "");

//                switch (keyboardStr)
//                {
//                    case "Telephone":
//                        keyboardType = InputTypes.ClassPhone;
//                        break;
//                    case "Email":
//                        keyboardType = InputTypes.TextVariationEmailAddress;
//                        break;
//                    case "Numeric":
//                        keyboardType = InputTypes.ClassNumber;
//                        break;
//                    case "Text":
//                        keyboardType = InputTypes.ClassText;
//                        break;
//                    case "Url":
//                        keyboardType = InputTypes.TextVariationUri;
//                        break;
//                    default:
//                        break;
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex);
//            }

//            return keyboardType;
//        }

//        /// <summary>
//        /// Gets the type of the keyboard return key.
//        /// </summary>
//        /// <returns>The keyboard return key type.</returns>
//        /// <param name="returnType">Return type.</param>
//        private ImeAction GetKeyboardReturnKeyType(ReturnType returnType)
//        {
//            ImeAction returnKeyType = ImeAction.ImeNull;

//            try
//            {
//                returnKeyType = (ImeAction)Enum.Parse(typeof(ImeAction), returnType.ToString());
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex);
//            }

//            return returnKeyType;
//        }

//        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
//        {
//            base.OnElementPropertyChanged(sender, e);

//            if (editText == null)
//            {
//                return;
//            }

//            if (e.PropertyName == nameof(MaterialTextbox.Text) || e.PropertyName == nameof(MaterialTextbox.PlaceHolderText))
//            {
//                UpdateText(((MaterialTextbox)sender).Text);
//            }
//        }

//        /// <summary>
//        /// Updates the text.
//        /// </summary>
//        /// <param name="text">Text.</param>
//        private void UpdateText(string text)
//        {
//            if (text == null)
//            {
//                return;
//            }

//            if (materialTextbox.Text != text)
//                materialTextbox.Text = text;

//            if (editText.Text != text)
//                editText.Text = text;

//            if (!string.IsNullOrWhiteSpace(text))
//            {
//                PerformTranslationYAnimation();
//            }
//            else
//            {
//                ReverseTranslationYAnimation();
//            }
//        }

//        public class OnDrawableTouchListener : Java.Lang.Object,  IOnTouchListener
//        {
//            private MaterialTextbox materialTextbox;

//            public OnDrawableTouchListener(MaterialTextbox MaterialTextbox)
//            {
//                materialTextbox = MaterialTextbox;
//            }

//            public bool OnTouch(global::Android.Views.View v, MotionEvent e)
//            {
//                try
//                {
//                    if (v is FormsEditText && e.Action == MotionEventActions.Down)
//                    {
//                        FormsEditText editText = (FormsEditText)v;


//                        if (e.RawX >= (editText.Right - editText.GetCompoundDrawables()[2].Bounds.Width()))
//                        {
//                            Toast.MakeText(v.Context, "icon", ToastLength.Short).Show();
//                            materialTextbox.TapPressCommand?.Execute(null);
//                            return true;
//                        }
//                    }
//                }
//                catch
//                {

//                }
//                return false;
//            }
//        }
//    }
//}
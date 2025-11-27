using System.Collections.Generic;
using Xamarin.Forms.Clinical6.Core.Helpers;
using Xamarin.Forms;
using System;

namespace Xamarin.Forms.Clinical6.UI.Controls
{
    public partial class ActionForm : ContentView
    {
        public ActionForm()
        {
            InitializeComponent();
        }

        public ActionForm(object[] args)
        {
           InitializeComponent();
        }

        public static readonly BindableProperty ImageUrlProperty = BindableProperty.Create(nameof(ImageUrl), typeof(string), typeof(ActionForm));

        public string ImageUrl
        {
            get => (string)GetValue(ImageUrlProperty);
            set => SetValue(ImageUrlProperty, value);
        }

        public ImageSource ImageUri => ImageSource.FromUri(new Uri(ImageUrl));

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(ActionForm));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty SubtitleProperty = BindableProperty.Create(nameof(Subtitle), typeof(string), typeof(ActionForm));

        public string Subtitle
        {
            get => (string)GetValue(SubtitleProperty);
            set => SetValue(SubtitleProperty, value);
        }


        public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(string), typeof(ActionForm));

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public static readonly BindableProperty HtmlDescriptionProperty = BindableProperty.Create(nameof(HtmlDescription), typeof(string), typeof(ActionForm));

        public string HtmlDescription
        {
            get => (string)GetValue(HtmlDescriptionProperty);
            set => SetValue(HtmlDescriptionProperty, value);
        }

        public static readonly BindableProperty ShowDescriptionProperty = BindableProperty.Create(nameof(ShowDescription), typeof(bool), typeof(ActionForm), true);

        public bool ShowDescription
        {
            get => (bool)GetValue(ShowDescriptionProperty);
            set => SetValue(ShowDescriptionProperty, value);
        }


        public static readonly BindableProperty ShowProgressProperty = BindableProperty.Create(nameof(ShowProgress), typeof(bool), typeof(ActionForm), false);

        public bool ShowProgress
        {
            get => (bool)GetValue(ShowProgressProperty);
            set => SetValue(ShowProgressProperty, value);
        }


        public static readonly BindableProperty ProgressProperty = BindableProperty.Create(nameof(Progress), typeof(double), typeof(ActionForm), 0.0);

        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }


        public static readonly BindableProperty FormBusyProperty = BindableProperty.Create(nameof(FormBusy), typeof(BusyTracker), typeof(ActionForm), new BusyTracker());

        public BusyTracker FormBusy
        {
            get => (BusyTracker)GetValue(FormBusyProperty);
            set => SetValue(FormBusyProperty, value);
        }


        public static readonly BindableProperty AllowScrollProperty = BindableProperty.Create(nameof(AllowScroll), typeof(bool), typeof(ActionForm), true,
            propertyChanged: CanScrollChanged);

        public bool AllowScroll
        {
            get => (bool)GetValue(AllowScrollProperty);
            set => SetValue(AllowScrollProperty, value);
        }


        public View FormContent
        {
            get => formContent.Content;
            set
            {
                formContent.Content = value;
                SetCanScroll();
            }
        }
        
        
        public static readonly BindableProperty ButtonsProperty = BindableProperty.Create(nameof(Buttons), typeof(IList<Button>), typeof(ActionForm), new List<Button>(),
            propertyChanged: ButtonsChanged);
            
            
            public static readonly BindableProperty ButtonProperty = BindableProperty.Create(nameof(Buttons), typeof(Button), typeof(ActionForm), null,
            propertyChanged: ButtonChanged);

        public Button ButtonDone
        {
            get => (Button)GetValue(ButtonProperty);
            set => SetValue(ButtonProperty, value);
        }

        public IList<Button> Buttons
        {
            get => (IList<Button>)GetValue(ButtonsProperty);
            set => SetValue(ButtonsProperty, value);
        }

        public static readonly BindableProperty buttonContinueProperty = BindableProperty.Create(nameof(ButtonContinue), typeof(IList<Button>), typeof(ActionForm), new List<Button>(),
            propertyChanged: ButtoncontinueChanged);

        public IList<Button> ButtonContinue
        {
            get => (IList<Button>)GetValue(buttonContinueProperty);
            set => SetValue(buttonContinueProperty, value);
        }

        private void SetCanScroll()
        {
            //if (formContent.Content == null) return;
            if (_actionForm.Content == null) return;

            if (AllowScroll)
            {
                TryAddScrollView();
            }
            else
            {
                TryRemoveScrollView();
            }
        }

        private void TryAddScrollView()
        {
            //var innerContent = formContent.Content;
            var innerContent = _actionForm.Content;
            if (innerContent is ScrollView) return;

            //formContent.Content = new ScrollView
            _actionForm.Content = new ScrollView
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Content = innerContent
            };
        }


        private void TryRemoveScrollView()
        {
            //if (formContent.Content is ScrollView innerScroll)
            if (_actionForm.Content is ScrollView innerScroll)
            {
                //formContent.Content = innerScroll.Content;
                _actionForm.Content = innerScroll.Content;
            }
        }


        private static void CanScrollChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var actionForm = (ActionForm)bindable;
            actionForm.SetCanScroll();
        }
        
        private static void ButtonChanged(BindableObject bindable, object oldValue, object newValue)
        {
        var newButton = newValue as Button;
            if (newButton == null) return;

            var form = (ActionForm)bindable;
            form.buttonContinue.Children.Clear();

            // Use a grid to evenly distribute the buttons horizontally
            /*
             * NOTE: We tried this with StackLayout and FillAndExpand, but there seems
             * to be an issue with that layout, and iOS buttons are not evenly sized.
             */
            var newColumns = new ColumnDefinitionCollection();
            var col = 0;
                newColumns.Add(new ColumnDefinition { Width = GridLength.Star });
                Grid.SetRow(newButton, 0);
                Grid.SetColumn(newButton, col++);

            form.buttonContinue.ColumnDefinitions = newColumns;

            form.buttonContinue.Children.Add(newButton);
            
            
            var lst = new List<Button>();
            lst.Add(newButton);
            form.Buttons = lst;
        }
        

        private static void ButtoncontinueChanged(BindableObject bindable, object oldValue, object newValue)
        {

            var newButtons = newValue as IList<Button>;
            if (newButtons == null) return;

            var form = (ActionForm)bindable;
            form.buttonContinue.Children.Clear();

            // Use a grid to evenly distribute the buttons horizontally
            /*
             * NOTE: We tried this with StackLayout and FillAndExpand, but there seems
             * to be an issue with that layout, and iOS buttons are not evenly sized.
             */
            var newColumns = new ColumnDefinitionCollection();
            var col = 0;
            foreach (var newButton in newButtons)
            {
                newColumns.Add(new ColumnDefinition { Width = GridLength.Star });
                Grid.SetRow(newButton, 0);
                Grid.SetColumn(newButton, col++);
            }

            form.buttonContinue.ColumnDefinitions = newColumns;

            foreach (var button in newButtons)
            {
                form.buttonContinue.Children.Add(button);
            }
        }

        private static void ButtonsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var newButtons = newValue as IList<Button>;
            if (newButtons == null) return;

            var form = (ActionForm)bindable;
            form.buttons.Children.Clear();

            // Use a grid to evenly distribute the buttons horizontally
            /*
             * NOTE: We tried this with StackLayout and FillAndExpand, but there seems
             * to be an issue with that layout, and iOS buttons are not evenly sized.
             */
            var newColumns = new ColumnDefinitionCollection();
            var col = 0;
            foreach (var newButton in newButtons)
            {
                newColumns.Add(new ColumnDefinition { Width = GridLength.Star });
                Grid.SetRow(newButton, 0);
                Grid.SetColumn(newButton, col++);
            }

            form.buttons.ColumnDefinitions = newColumns;

            foreach (var button in newButtons)
            {
                form.buttons.Children.Add(button);
            }
        }
    }
}

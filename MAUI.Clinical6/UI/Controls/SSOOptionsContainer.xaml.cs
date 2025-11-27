using System.Collections;

namespace Xamarin.Forms.Clinical6.UI.Controls
{
    public partial class SSOOptionsContainer : ContentView
    {
        public SSOOptionsContainer()
        {
            InitializeComponent();
            box.Children.Clear();
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(SSOOptionsContainer), default(IList), BindingMode.TwoWay,
            propertyChanging: ItemsSourcePropertyChanging, propertyChanged: ItemsSourcePropertyPropertyChanged);

        public IList ItemsSource
        {
            get
            {
                return (IList)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        /// <summary>
        /// Handle when the source property is changing
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void ItemsSourcePropertyChanging(BindableObject bindable, object oldValue, object newValue)
        {
            ((SSOOptionsContainer)bindable).ItemsSourceChanging();
        }

        /// <summary>
        ///  Handle when the source property is changed
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void ItemsSourcePropertyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((SSOOptionsContainer)bindable).ItemsSourceChanged(bindable, oldValue as IList, newValue as IList);
        }

        /// <summary>
        /// Itemses the source changed.
        /// </summary>
        /// <param name="bindable">Bindable.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private void ItemsSourceChanged(BindableObject bindable, IList oldValue, IList newValue)
        {
            if (ItemsSource == null)
            {
                return;
            }

            var notifyCollection = newValue as IEnumerable;

            if (notifyCollection != null)
            {
                int MarginBox = 0;
                foreach (var newItem in notifyCollection)
                {
                    box.Children.Add(new SSOOptionsController() { BindingContext = newItem, Margin = new Thickness(0, MarginBox, 0, 0) });

                    MarginBox = 10;
                }
            }
        }

        /// <summary>
        /// Itemses the source changing.
        /// </summary>
        private void ItemsSourceChanging()
        {
            if (ItemsSource == null)
                return;
        }
    }
}
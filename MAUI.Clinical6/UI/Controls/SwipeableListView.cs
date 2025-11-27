using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms.Clinical6.Core.Models;

namespace Xamarin.Forms.Clinical6.UI.Controls
{
    public class SwipeableListView : View
    {
        public static readonly BindableProperty ItemsProperty =
            BindableProperty.Create(nameof(Items), typeof(IEnumerable<Swipeable>), typeof(SwipeableListView),
            new List<Swipeable>(), propertyChanged: OnItemsChanged, defaultBindingMode: BindingMode.TwoWay);

        public IEnumerable<Swipeable> Items
        {
            get { return (IEnumerable<Swipeable>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly BindableProperty LeftButtonEnabledProperty =
            BindableProperty.Create(nameof(LeftButtonEnabled), typeof(bool), typeof(SwipeableListView), true);

        public bool LeftButtonEnabled
        {
            get { return (bool)GetValue(LeftButtonEnabledProperty); }
            set { SetValue(LeftButtonEnabledProperty, value); }
        }

        public static readonly BindableProperty LeftButtonColorProperty =
            BindableProperty.Create(nameof(LeftButtonColor), typeof(string), typeof(SwipeableListView), "#007ac9");

        public string LeftButtonColor
        {
            get { return (string)GetValue(LeftButtonColorProperty); }
            set { SetValue(LeftButtonColorProperty, value); }
        }

        public static readonly BindableProperty RightButtonColorProperty =
            BindableProperty.Create(nameof(RightButtonColor), typeof(string), typeof(SwipeableListView), "#004aa9");

        public string RightButtonColor
        {
            get { return (string)GetValue(RightButtonColorProperty); }
            set { SetValue(RightButtonColorProperty, value); }
        }

        public static readonly BindableProperty RightButtonEnabledProperty =
            BindableProperty.Create(nameof(RightButtonEnabled), typeof(bool), typeof(SwipeableListView), true);

        public bool RightButtonEnabled
        {
            get { return (bool)GetValue(RightButtonEnabledProperty); }
            set { SetValue(RightButtonEnabledProperty, value); }
        }

        public static readonly BindableProperty SelectedItemCommandProperty =
            BindableProperty.Create(nameof(SelectedItemCommand), typeof(ICommand), typeof(SwipeableListView), null);

        public ICommand SelectedItemCommand
        {
            get { return (ICommand)GetValue(SelectedItemCommandProperty); }
            set { SetValue(SelectedItemCommandProperty, value); }
        }

        public static readonly BindableProperty LeftButtonCommandProperty =
            BindableProperty.Create(nameof(LeftButtonCommand), typeof(ICommand), typeof(SwipeableListView), null);

        public ICommand LeftButtonCommand
        {
            get { return (ICommand)GetValue(LeftButtonCommandProperty); }
            set { SetValue(LeftButtonCommandProperty, value); }
        }

        public static readonly BindableProperty RightButtonCommandProperty =
            BindableProperty.Create(nameof(RightButtonCommand), typeof(ICommand), typeof(SwipeableListView), null);

        public ICommand RightButtonCommand
        {
            get { return (ICommand)GetValue(RightButtonCommandProperty); }
            set { SetValue(RightButtonCommandProperty, value); }
        }

        private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var swipeableListView = bindable as SwipeableListView;

            if (ItemsChangedEvent != null)
                ItemsChangedEvent.Invoke(swipeableListView, (IEnumerable<Swipeable>)newValue);

            UpdateEmptyView(swipeableListView);
        }

        public static event EventHandler<IEnumerable<Swipeable>> ItemsChangedEvent;

        public static readonly BindableProperty EmptyViewProperty =
           BindableProperty.Create(nameof(EmptyView), typeof(View), typeof(SwipeableListView), null, propertyChanged: OnEmptyViewChanged);

        public View EmptyView
        {
            get { return (View)GetValue(EmptyViewProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        private static void OnEmptyViewChanged(BindableObject bindable, object oldValue, object newValue)
        {
            UpdateEmptyView((SwipeableListView)bindable);
        }

        private static void UpdateEmptyView(SwipeableListView swipeableListView)
        {
            if (swipeableListView?.EmptyView != null)
            {
                if (swipeableListView.Items == null || !swipeableListView.Items.Any())
                {
                    swipeableListView.EmptyView.IsVisible = true;
                }
                else
                {
                    swipeableListView.EmptyView.IsVisible = false;
                }
            }
        }
    }
}
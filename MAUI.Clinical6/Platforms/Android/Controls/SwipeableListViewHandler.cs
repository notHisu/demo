using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Clinical6.Core.Models;
using Xamarin.Forms.Clinical6.UI.Controls;
using Button = Android.Widget.Button;
using droid = Android;
using Paint = Android.Graphics.Paint;
using RectF = Android.Graphics.RectF;

namespace Xamarin.Forms.Clinical6.Maui.Handlers
{
    public class SwipeableListViewHandler : ViewHandler<SwipeableListView, RecyclerView>
    {
        private RecyclerView recyclerView;
        private ItemAdapter adapter;
        private SwipeController swipeController;
        private SwipeControllerActions swipeControllerActions;

        public static IPropertyMapper<SwipeableListView, SwipeableListViewHandler> Mapper =
    new PropertyMapper<SwipeableListView, SwipeableListViewHandler>(ViewMapper);

        public SwipeableListViewHandler() : base(Mapper)
        {
        }

        protected override RecyclerView CreatePlatformView()
        {
            recyclerView = new RecyclerView(Context);

            swipeControllerActions = new SwipeControllerActions();
            swipeControllerActions.LeftButtonEvent += SwipeControllerActions_LeftButtonEvent;
            swipeControllerActions.RightButtonEvent += SwipeControllerActions_RightButtonEvent;

            swipeController = new SwipeController(swipeControllerActions);

            var itemTouchHelper = new ItemTouchHelper(swipeController);
            itemTouchHelper.AttachToRecyclerView(recyclerView);
            recyclerView.AddItemDecoration(new CustomDecoration(swipeController));

            var layoutManager = new LinearLayoutManager(Context);
            recyclerView.SetLayoutManager(layoutManager);

            return recyclerView;
        }

        protected override void ConnectHandler(RecyclerView platformView)
        {
            base.ConnectHandler(platformView);

            SetAdapter(VirtualView.Items);
            SetContextMenuButtons();

            SwipeableListView.ItemsChangedEvent += SwipeableListView_ItemsChangedEvent;
        }

        protected override void DisconnectHandler(RecyclerView platformView)
        {
            base.DisconnectHandler(platformView);

            SwipeableListView.ItemsChangedEvent -= SwipeableListView_ItemsChangedEvent;
        }

        private void SwipeableListView_ItemsChangedEvent(object sender, IEnumerable<Swipeable> e)
        {
            SetAdapter(e);
            SetContextMenuButtons();
        }

        private void SetAdapter(IEnumerable<Swipeable> items)
        {
            if (items == null) return;

            adapter = new ItemAdapter(items, swipeController, VirtualView);
            recyclerView.SetAdapter(adapter);
        }

        private void SetContextMenuButtons()
        {
            if (VirtualView == null) return;

            swipeController.LeftButtonEnabled = VirtualView.LeftButtonEnabled;
            swipeController.LeftButtonColor = droid.Graphics.Color.ParseColor(VirtualView.LeftButtonColor);

            swipeController.RightButtonEnabled = VirtualView.RightButtonEnabled;
            swipeController.RightButtonColor = droid.Graphics.Color.ParseColor(VirtualView.RightButtonColor);
        }

        private void SwipeControllerActions_LeftButtonEvent(object sender, int position)
        {
            if (position < 0) return;
            var item = VirtualView.Items.ToList()[position];
            VirtualView.LeftButtonCommand?.Execute(item);
        }

        private void SwipeControllerActions_RightButtonEvent(object sender, int position)
        {
            if (position < 0) return;
            var item = VirtualView.Items.ToList()[position];
            VirtualView.RightButtonCommand?.Execute(item);
        }

        // ==================== INNER CLASSES ====================

        public class CustomDecoration : RecyclerView.ItemDecoration
        {
            private SwipeController _swipeController;

            public CustomDecoration(SwipeController swipeController)
            {
                _swipeController = swipeController;
            }

            public override void OnDraw(Canvas c, RecyclerView parent, RecyclerView.State state)
            {
                _swipeController.OnDraw(c);
            }
        }

        public class SwipeControllerActions
        {
            public event EventHandler<int> LeftButtonEvent;
            public event EventHandler<int> RightButtonEvent;

            public void OnLeftClicked(int position) => LeftButtonEvent?.Invoke(this, position);
            public void OnRightClicked(int position) => RightButtonEvent?.Invoke(this, position);
        }

        public enum ButtonState
        {
            GONE = 0,
            LEFT_VISIBLE,
            RIGHT_VISIBLE
        }

        public class SwipeController : ItemTouchHelper.Callback
        {
            public Bitmap LeftButtonImage { get; set; }
            public Bitmap RightButtonImage { get; set; }

            private droid.Graphics.Color leftButtonColor = droid.Graphics.Color.Blue;
            public droid.Graphics.Color LeftButtonColor { get => leftButtonColor; set => leftButtonColor = value; }

            public string LeftButtonText { get; set; } = "EditText";

            private droid.Graphics.Color rightButtonColor = droid.Graphics.Color.Red;
            public droid.Graphics.Color RightButtonColor { get => rightButtonColor; set => rightButtonColor = value; }

            public string RightButtonText { get; set; } = "DeleteText";

            public ButtonState CurrentButtonState => buttonShowedState;

            public bool LeftButtonEnabled { get; set; }
            public bool RightButtonEnabled { get; set; }

            private bool swipeBack = false;
            private ButtonState buttonShowedState = ButtonState.GONE;
            private float buttonWidth = 300;
            private RectF buttonInstance = null;
            private RecyclerView.ViewHolder currentItemViewHolder = null;
            private SwipeControllerActions buttonsActions = null;
            private Canvas canvas;
            private RecyclerView.ViewHolder viewHolder;
            private RecyclerView recyclerView;
            private float dX;
            private float dY;
            private int actionState;
            private bool isCurrentlyActive;

            public SwipeController(SwipeControllerActions buttonsActions)
            {
                this.buttonsActions = buttonsActions;
            }

            public override int GetMovementFlags(RecyclerView p0, RecyclerView.ViewHolder p1)
            {
                return MakeMovementFlags(0, GetSwipeFlags());
            }

            private int GetSwipeFlags()
            {
                if (LeftButtonEnabled && RightButtonEnabled) return ItemTouchHelper.Left | ItemTouchHelper.Right;
                if (LeftButtonEnabled) return ItemTouchHelper.Right;
                if (RightButtonEnabled) return ItemTouchHelper.Left;
                return 0;
            }

            public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target) => false;

            public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction) { }

            public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX,
                float dY, int actionState, bool isCurrentlyActive)
            {
                if (actionState == ItemTouchHelper.ActionStateSwipe)
                {
                    if (buttonShowedState != ButtonState.GONE)
                    {
                        if (buttonShowedState == ButtonState.LEFT_VISIBLE) dX = Math.Max(dX, buttonWidth);
                        if (buttonShowedState == ButtonState.RIGHT_VISIBLE) dX = Math.Min(dX, -buttonWidth);
                        base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
                    }
                    else SetTouchListener(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
                }

                if (buttonShowedState == ButtonState.GONE)
                {
                    currentItemViewHolder = null;
                    base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
                }

                currentItemViewHolder = viewHolder;
            }

            public override int ConvertToAbsoluteDirection(int flags, int layoutDirection)
            {
                if (swipeBack)
                {
                    swipeBack = buttonShowedState != ButtonState.GONE;
                    return 0;
                }
                return base.ConvertToAbsoluteDirection(flags, layoutDirection);
            }

            private void SetTouchListener(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX,
                float dY, int actionState, bool isCurrentlyActive)
            {
                SetLocalValues(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
                recyclerView.Touch -= RecyclerView_Touch;
                recyclerView.Touch += RecyclerView_Touch;
            }

            private void SetLocalValues(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX,
                float dY, int actionState, bool isCurrentlyActive)
            {
                this.canvas = c;
                this.recyclerView = recyclerView;
                this.viewHolder = viewHolder;
                this.dX = dX;
                this.dY = dY;
                this.actionState = actionState;
                this.isCurrentlyActive = isCurrentlyActive;
            }

            private void RecyclerView_Touch(object sender, droid.Views.View.TouchEventArgs e)
            {
                if (buttonShowedState == ButtonState.LEFT_VISIBLE || buttonShowedState == ButtonState.RIGHT_VISIBLE)
                    return;

                swipeBack = e.Event.Action == MotionEventActions.Cancel || e.Event.Action == MotionEventActions.Up;

                if (swipeBack)
                {
                    if (dX < -buttonWidth) buttonShowedState = ButtonState.RIGHT_VISIBLE;
                    else if (dX > buttonWidth) buttonShowedState = ButtonState.LEFT_VISIBLE;

                    if (buttonShowedState != ButtonState.GONE)
                    {
                        SetTouchDownListener(canvas, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
                        SetItemsClickable(recyclerView, false);
                    }
                }
                e.Handled = false;
            }

            private void SetTouchDownListener(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX,
                float dY, int actionState, bool isCurrentlyActive)
            {
                SetLocalValues(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
                recyclerView.Touch -= RecyclerView_TouchDown;
                recyclerView.Touch += RecyclerView_TouchDown;
            }

            private void RecyclerView_TouchDown(object sender, droid.Views.View.TouchEventArgs e)
            {
                if (e.Event.Action == MotionEventActions.Down)
                    SetTouchUpListener(canvas, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);

                e.Handled = false;
            }

            private void SetTouchUpListener(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX,
                float dY, int actionState, bool isCurrentlyActive)
            {
                SetLocalValues(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
                recyclerView.Touch -= RecyclerView_Touchup;
                recyclerView.Touch += RecyclerView_Touchup;
            }

            private void RecyclerView_Touchup(object sender, droid.Views.View.TouchEventArgs e)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (e.Event.Action == MotionEventActions.Up)
                    {
                        base.OnChildDraw(canvas, recyclerView, viewHolder, 0F, dY, actionState, isCurrentlyActive);

                        if (buttonsActions != null && buttonInstance != null && buttonInstance.Contains(e.Event.GetX(), e.Event.GetY()))
                        {
                            if (buttonShowedState == ButtonState.LEFT_VISIBLE)
                                buttonsActions.OnLeftClicked(viewHolder.AdapterPosition);
                            else if (buttonShowedState == ButtonState.RIGHT_VISIBLE)
                                buttonsActions.OnRightClicked(viewHolder.AdapterPosition);
                        }

                        SetItemsClickable(recyclerView, true);
                        swipeBack = false;
                        buttonShowedState = ButtonState.GONE;
                        currentItemViewHolder = null;
                    }
                    else if (e.Event.Action == MotionEventActions.Cancel | e.Event.Action == MotionEventActions.Move)
                    {
                        buttonShowedState = ButtonState.GONE;
                        currentItemViewHolder = null;
                        base.OnChildDraw(canvas, recyclerView, viewHolder, 0F, dY, actionState, isCurrentlyActive);
                    }
                    e.Handled = false;
                });
            }

            private void DrawButtons(Canvas c, RecyclerView.ViewHolder viewHolder)
            {
                if (viewHolder.AdapterPosition < 0) return;
                if (viewHolder.AdapterPosition >= ((ItemAdapter)recyclerView.GetAdapter()).ItemCount) return;

                var item = ((ItemAdapter)recyclerView.GetAdapter()).Items[viewHolder.AdapterPosition];
                droid.Views.View itemView = viewHolder.ItemView;
                Paint p = new Paint();
                float buttonWidthWithoutPadding = buttonWidth - 20;
                float buttonHeightPadding = 13;
                float corners = 0;

                RectF leftButton = new RectF(itemView.Left, itemView.Top + buttonHeightPadding,
                    itemView.Left + buttonWidthWithoutPadding, itemView.Bottom - buttonHeightPadding);
                p.Color = LeftButtonColor;
                c.DrawRoundRect(leftButton, corners, corners, p);
                DrawText(item.LeftButtonText, c, leftButton, p);

                RectF rightButton = new RectF(itemView.Right - buttonWidthWithoutPadding, itemView.Top + buttonHeightPadding,
                    itemView.Right, itemView.Bottom - buttonHeightPadding);
                p.Color = RightButtonColor;
                c.DrawRoundRect(rightButton, corners, corners, p);
                DrawText(item.RightButtonText, c, rightButton, p);

                if (buttonShowedState == ButtonState.LEFT_VISIBLE) buttonInstance = leftButton;
                else if (buttonShowedState == ButtonState.RIGHT_VISIBLE) buttonInstance = rightButton;
            }

            private void DrawText(string text, Canvas c, RectF button, Paint p)
            {
                float textSize = 45;
                p.Color = droid.Graphics.Color.White;
                p.AntiAlias = true;
                p.TextSize = textSize;

                float textWidth = p.MeasureText(text);
                float valueX = button.CenterX() - (textWidth / 2);
                float valueY = button.CenterY() + (textSize / 2);
                c.DrawText(text, valueX, valueY, p);
            }

            private void SetItemsClickable(RecyclerView recyclerView, bool isClickable)
            {
                for (int i = 0; i < recyclerView.ChildCount; ++i)
                    recyclerView.GetChildAt(i).Clickable = isClickable;
            }

            public void OnDraw(Canvas c)
            {
                if (currentItemViewHolder == null) return;
                DrawButtons(c, currentItemViewHolder);
            }
        }

        public class ItemViewHolder : RecyclerView.ViewHolder
        {
            public TextView Title { get; private set; }
            public TextView Message { get; private set; }
            public Button NewInfoIndicatorButton { get; private set; }
            public TextView Date { get; private set; }

            public ItemViewHolder(droid.Views.View itemView) : base(itemView)
            {
                Title = itemView.FindViewById<TextView>(MAUI.Clinical6.Resource.Id.text_title);
                Message = itemView.FindViewById<TextView>(MAUI.Clinical6.Resource.Id.text_message);
                Date = itemView.FindViewById<TextView>(MAUI.Clinical6.Resource.Id.text_date);
                NewInfoIndicatorButton = itemView.FindViewById<Button>(MAUI.Clinical6.Resource.Id.button_red_circle);
            }
        }

        public class ItemAdapter : RecyclerView.Adapter
        {
            private SwipeController swipeController;
            public List<Swipeable> Items { get; }
            private readonly SwipeableListView swipeableListView;

            public ItemAdapter(IEnumerable<Swipeable> items, SwipeController swipeController, SwipeableListView swipeableListView)
            {
                Items = new List<Swipeable>(items);
                this.swipeController = swipeController;
                this.swipeableListView = swipeableListView;
            }

            public override int ItemCount => Items.Count;

            public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
            {
                var viewHolder = holder as ItemViewHolder;
                var swipeable = Items[position];

                viewHolder.Title.Text = swipeable.Title;
                viewHolder.Message.Text = swipeable.Description;
                viewHolder.Date.Text = swipeable.ExtraInfo;

                if (!swipeable.IsNewInfoAvailable) viewHolder.NewInfoIndicatorButton.Visibility = ViewStates.Invisible;

                viewHolder.ItemView.Click += (s, e) =>
                {
                    if (swipeableListView.SelectedItemCommand == null) return;
                    if (swipeController.CurrentButtonState != ButtonState.GONE) return;
                    swipeableListView.SelectedItemCommand.Execute(Items[position]);
                };
            }

            public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
            {
                var itemView = LayoutInflater.From(parent.Context).Inflate(MAUI.Clinical6.Resource.Layout.CustomListViewRendererCell, parent, false);
                return new ItemViewHolder(itemView);
            }
        }
    }
}



//using Android.Content;
//using Android.Graphics;
//using Android.Views;
//using Android.Widget;
//using AndroidX.RecyclerView.Widget;
//using Microsoft.Maui.Controls.Compatibility.Platform.Android;
//using Microsoft.Maui.Controls.Platform;
//using Microsoft.Maui.Platform;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Xamarin.Forms;
//using Xamarin.Forms.Clinical6.Android.Renderers;
//using Xamarin.Forms.Clinical6.Core.Helpers;
//using Xamarin.Forms.Clinical6.Core.Models;
//using Xamarin.Forms.Clinical6.UI.Controls;
//using droid = Android;
//using Paint = Android.Graphics.Paint;
//using RectF = Android.Graphics.RectF;

//namespace Xamarin.Forms.Clinical6.Android.Renderers
//{
//    public class CustomDecoration : RecyclerView.ItemDecoration
//    {
//        private SwipeController _swipeController;

//        public CustomDecoration(SwipeController swipeController)
//        {
//            _swipeController = swipeController;
//        }

//        public override void OnDraw(Canvas c, RecyclerView parent, RecyclerView.State state)
//        {
//            _swipeController.OnDraw(c);
//        }
//    }

//    public class SwipeControllerActions
//    {
//        public event EventHandler<int> LeftButtonEvent;
//        public event EventHandler<int> RightButtonEvent;

//        public void OnLeftClicked(int position)
//        {
//            if (LeftButtonEvent != null)
//                LeftButtonEvent.Invoke(this, position);
//        }

//        public void OnRightClicked(int position)
//        {
//            if (RightButtonEvent != null)
//                RightButtonEvent.Invoke(this, position);
//        }
//    }

//    public enum ButtonState
//    {
//        GONE = 0,
//        LEFT_VISIBLE,
//        RIGHT_VISIBLE
//    }

//    public class SwipeController : ItemTouchHelper.Callback
//    {
//        public Bitmap LeftButtonImage { get; set; }

//        public Bitmap RightButtonImage { get; set; }

//        private droid.Graphics.Color leftButtonColor = droid.Graphics.Color.Blue;
//        public droid.Graphics.Color LeftButtonColor
//        {
//            get { return leftButtonColor; }
//            set { leftButtonColor = value; }
//        }

//        public string LeftButtonText { get; set; } = "EditText".Localized();

//        private droid.Graphics.Color rightButtonColor = droid.Graphics.Color.Red;
//        public droid.Graphics.Color RightButtonColor
//        {
//            get { return rightButtonColor; }
//            set { rightButtonColor = value; }
//        }

//        public string RightButtonText { get; set; } = "DeleteText".Localized();

//        public ButtonState CurrentButtonState
//        {
//            get { return buttonShowedState; }
//        }

//        public bool LeftButtonEnabled { get; set; }
//        public bool RightButtonEnabled { get; set; }

//        private bool swipeBack = false;
//        private ButtonState buttonShowedState = ButtonState.GONE;
//        private float buttonWidth = 300;
//        private RectF buttonInstance = null;
//        private RecyclerView.ViewHolder currentItemViewHolder = null;
//        private SwipeControllerActions buttonsActions = null;
//        private Canvas canvas;
//        private RecyclerView.ViewHolder viewHolder;
//        private RecyclerView recyclerView;
//        private float dX;
//        private float dY;
//        private int actionState;
//        private bool isCurrentlyActive;

//        public SwipeController(SwipeControllerActions buttonsActions)
//        {
//            this.buttonsActions = buttonsActions;
//        }

//        public override int GetMovementFlags(RecyclerView p0, RecyclerView.ViewHolder p1)
//        {
//            return MakeMovementFlags(0, GetSwipeFlags());
//        }

//        private int GetSwipeFlags()
//        {
//            int swipeflags = 0;

//            if (LeftButtonEnabled && RightButtonEnabled)
//            {
//                swipeflags = ItemTouchHelper.Left | ItemTouchHelper.Right;
//            }
//            else if (LeftButtonEnabled)
//            {
//                swipeflags = ItemTouchHelper.Right;
//            }
//            else if (RightButtonEnabled)
//            {
//                swipeflags = ItemTouchHelper.Left;
//            }

//            return swipeflags;
//        }

//        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
//        {
//            return false;
//        }

//        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
//        {
//        }

//        public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX,
//            float dY, int actionState, bool isCurrentlyActive)
//        {
//            if (actionState == ItemTouchHelper.ActionStateSwipe)
//            {
//                if (buttonShowedState != ButtonState.GONE)
//                {
//                    if (buttonShowedState == ButtonState.LEFT_VISIBLE) dX = Math.Max(dX, buttonWidth);
//                    if (buttonShowedState == ButtonState.RIGHT_VISIBLE) dX = Math.Min(dX, -buttonWidth);
//                    base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
//                }
//                else
//                {
//                    SetTouchListener(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
//                }
//            }

//            if (buttonShowedState == ButtonState.GONE)
//            {
//                currentItemViewHolder = null;
//                base.OnChildDraw(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
//            }

//            currentItemViewHolder = viewHolder;
//        }

//        public override int ConvertToAbsoluteDirection(int flags, int layoutDirection)
//        {
//            if (swipeBack)
//            {
//                swipeBack = buttonShowedState != ButtonState.GONE;
//                return 0;
//            }

//            return base.ConvertToAbsoluteDirection(flags, layoutDirection);
//        }

//        private void SetTouchListener(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX,
//            float dY, int actionState, bool isCurrentlyActive)
//        {
//            SetLocalValues(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
//            recyclerView.Touch -= RecyclerView_Touch;
//            recyclerView.Touch += RecyclerView_Touch;
//        }

//        private void SetLocalValues(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX,
//            float dY, int actionState, bool isCurrentlyActive)
//        {
//            this.canvas = c;
//            this.recyclerView = recyclerView;
//            this.viewHolder = viewHolder;
//            this.dX = dX;
//            this.dY = dY;
//            this.actionState = actionState;
//            this.isCurrentlyActive = isCurrentlyActive;
//        }

//        private void RecyclerView_Touch(object sender, droid.Views.View.TouchEventArgs e)
//        {
//            if (buttonShowedState == ButtonState.LEFT_VISIBLE || buttonShowedState == ButtonState.RIGHT_VISIBLE)
//                return;

//            swipeBack = e.Event.Action == MotionEventActions.Cancel || e.Event.Action == MotionEventActions.Up;

//            if (swipeBack)
//            {
//                if (dX < -buttonWidth) buttonShowedState = ButtonState.RIGHT_VISIBLE;
//                else if (dX > buttonWidth) buttonShowedState = ButtonState.LEFT_VISIBLE;

//                if (buttonShowedState != ButtonState.GONE)
//                {
//                    SetTouchDownListener(this.canvas, this.recyclerView, this.viewHolder,
//                                         this.dX, this.dY, this.actionState, this.isCurrentlyActive);
//                    SetItemsClickable(recyclerView, false);
//                }
//            }
//            e.Handled = false;
//        }

//        private void SetTouchDownListener(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX,
//            float dY, int actionState, bool isCurrentlyActive)
//        {
//            SetLocalValues(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
//            recyclerView.Touch -= RecyclerView_TouchDown;
//            recyclerView.Touch += RecyclerView_TouchDown;
//        }

//        private void RecyclerView_TouchDown(object sender, droid.Views.View.TouchEventArgs e)
//        {
//            if (e.Event.Action == MotionEventActions.Down)
//            {
//                SetTouchUpListener(this.canvas, this.recyclerView, this.viewHolder, this.dX, this.dY, this.actionState, this.isCurrentlyActive);
//            }

//            e.Handled = false;
//        }

//        private void SetTouchUpListener(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX,
//            float dY, int actionState, bool isCurrentlyActive)
//        {
//            SetLocalValues(c, recyclerView, viewHolder, dX, dY, actionState, isCurrentlyActive);
//            recyclerView.Touch -= RecyclerView_Touchup;
//            recyclerView.Touch += RecyclerView_Touchup;
//        }

//        private void RecyclerView_Touchup(object sender, droid.Views.View.TouchEventArgs e)
//        {
//            MainThread.BeginInvokeOnMainThread(() =>
//            {
//                if (e.Event.Action == MotionEventActions.Up)
//                {
//                    base.OnChildDraw(this.canvas, this.recyclerView, this.viewHolder, 0F, dY, actionState, isCurrentlyActive);

//                    if (buttonsActions != null && buttonInstance != null && buttonInstance.Contains(e.Event.GetX(), e.Event.GetY()))
//                    {
//                        if (buttonShowedState == ButtonState.LEFT_VISIBLE)
//                        {
//                            buttonsActions.OnLeftClicked(this.viewHolder.AdapterPosition);
//                        }
//                        else if (buttonShowedState == ButtonState.RIGHT_VISIBLE)
//                        {
//                            buttonsActions.OnRightClicked(this.viewHolder.AdapterPosition);
//                        }
//                    }

//                    SetItemsClickable(recyclerView, true);
//                    swipeBack = false;

//                    buttonShowedState = ButtonState.GONE;
//                    currentItemViewHolder = null;
//                }
//                else if (e.Event.Action == MotionEventActions.Cancel | e.Event.Action == MotionEventActions.Move)
//                {
//                    buttonShowedState = ButtonState.GONE;
//                    currentItemViewHolder = null;
//                    base.OnChildDraw(this.canvas, this.recyclerView, this.viewHolder, 0F, dY, actionState, isCurrentlyActive);
//                }

//                e.Handled = false;
//            });
//        }

//        private void DrawButtons(Canvas c, RecyclerView.ViewHolder viewHolder)
//        {
//            float buttonWidthWithoutPadding = buttonWidth - 20;
//            float buttonHeightPadding = 13;
//            float corners = 0;

//            droid.Views.View itemView = viewHolder.ItemView;

//            var adapter = (ItemAdapter)recyclerView.GetAdapter();
//            if (viewHolder.AdapterPosition < 0)
//                return;

//            if (viewHolder.AdapterPosition == adapter.ItemCount)
//                return;

//            var currentRowItem = adapter.Items[viewHolder.AdapterPosition];
//            Paint p = new Paint();

//            RectF leftButton = new RectF(itemView.Left, itemView.Top + buttonHeightPadding, itemView.Left + buttonWidthWithoutPadding, itemView.Bottom - buttonHeightPadding);
//            p.Color = LeftButtonColor;
//            c.DrawRoundRect(leftButton, corners, corners, p);
//            DrawText(currentRowItem.LeftButtonText, c, leftButton, p);

//            float valueX = leftButton.CenterX() - (itemView.Left + 35);
//            float valueY = itemView.Top + buttonHeightPadding + 65;

//            var leftContextMenuImage = GetContextMenuImage(currentRowItem.LeftButtonImage);
//            if (leftContextMenuImage != null)
//            {
//                c.DrawBitmap(leftContextMenuImage, valueX, valueY, p);
//                //.c.DrawBitmap(leftContextMenuImage, itemView.Left, itemView.Top + buttonHeightPadding, p);
//            }

//            RectF rightButton = new RectF(itemView.Right - buttonWidthWithoutPadding, itemView.Top + buttonHeightPadding, itemView.Right,
//                                          itemView.Bottom - buttonHeightPadding);
//            p.Color = RightButtonColor;
//            c.DrawRoundRect(rightButton, corners, corners, p);
//            this.DrawText(currentRowItem.RightButtonText, c, rightButton, p);

//            valueX = itemView.Right - buttonWidthWithoutPadding + 120;
//            valueY = itemView.Top + buttonHeightPadding + 65;
//            var rightContextMenuImage = GetContextMenuImage(currentRowItem.RightButtonImage);

//            if (rightContextMenuImage != null)
//            {
//                c.DrawBitmap(rightContextMenuImage, valueX, valueY, p);
//                //c.DrawBitmap(rightContextMenuImage, itemView.Right - buttonWidthWithoutPadding, itemView.Top + buttonHeightPadding, p);
//            }

//            buttonInstance = null;

//            if (buttonShowedState == ButtonState.LEFT_VISIBLE)
//            {
//                buttonInstance = leftButton;
//            }
//            else if (buttonShowedState == ButtonState.RIGHT_VISIBLE)
//            {
//                buttonInstance = rightButton;
//            }
//        }

//        private Bitmap GetContextMenuImage(string imageName)
//        {
//            Bitmap bitmap = null;

//            try
//            {
//                int drawableId = recyclerView.Resources.GetIdentifier(imageName, "drawable", recyclerView.Context.PackageName);
//                bitmap = BitmapFactory.DecodeResource(recyclerView.Context.Resources, drawableId);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex);
//            }

//            return bitmap;
//        }

//        private void DrawText(string text, Canvas c, RectF button, Paint p)
//        {
//            float textSize = 45;
//            p.Color = droid.Graphics.Color.White;
//            p.AntiAlias = true;
//            p.TextSize = textSize;
//            var splitText = text.Split(" ");
//            float textWidth;

//            if (splitText.Count() == 2)
//            {
//                var firstText = splitText.FirstOrDefault();
//                var secondText = splitText.LastOrDefault();

//                textWidth = p.MeasureText(firstText);
//                float valueX = button.CenterX() - (textWidth / 2);
//                float valueY = button.CenterY() + (textSize / 2);

//                c.DrawText(firstText, valueX, valueY, p);

//                textWidth = p.MeasureText(secondText);
//                valueX = button.CenterX() - (textWidth / 2);

//                c.DrawText(secondText, valueX, valueY + textSize, p);
//            }
//            else
//            {
//                textWidth = p.MeasureText(text);
//                float valueX = button.CenterX() - (textWidth / 2);
//                float valueY = button.CenterY() + (textSize / 2);
//                c.DrawText(text, valueX, valueY, p);
//            }
//        }

//        private void SetItemsClickable(RecyclerView recyclerView, bool isClickable)
//        {
//            for (int i = 0; i < recyclerView.ChildCount; ++i)
//            {
//                recyclerView.GetChildAt(i).Clickable = isClickable;
//            }
//        }

//        public void OnDraw(Canvas c)
//        {
//            if (currentItemViewHolder == null)
//                return;

//            DrawButtons(c, currentItemViewHolder);
//        }
//    }

//    public class CustomListViewRenderer : ViewRenderer<SwipeableListView, LinearLayout>
//    {
//        private SwipeableListView swipeableListView;
//        private ItemAdapter adapter;
//        private RecyclerView recyclerView;
//        private SwipeController swipeController;
//        private SwipeControllerActions swipeControllerActions;

//        public CustomListViewRenderer(Context context) : base(context)
//        {
//            LayoutInflater inflater = (LayoutInflater)Context.GetSystemService(Context.LayoutInflaterService);
//            droid.Views.View view = inflater.Inflate(MAUI.Clinical6.Resource.Layout.CustomListViewLayout, null, true);
//            recyclerView = view.FindViewById<RecyclerView>(MAUI.Clinical6.Resource.Id.recyclerView);
//            recyclerView.RemoveFromParent();

//            swipeControllerActions = new SwipeControllerActions();

//            swipeControllerActions.LeftButtonEvent -= SwipeControllerActions_LeftButtonEvent;
//            swipeControllerActions.LeftButtonEvent += SwipeControllerActions_LeftButtonEvent;

//            swipeControllerActions.RightButtonEvent -= SwipeControllerActions_RightButtonEvent;
//            swipeControllerActions.RightButtonEvent += SwipeControllerActions_RightButtonEvent;

//            swipeController = new SwipeController(swipeControllerActions);

//            ItemTouchHelper itemTouchhelper = new ItemTouchHelper(swipeController);
//            itemTouchhelper.AttachToRecyclerView(recyclerView);
//            recyclerView.AddItemDecoration(new CustomDecoration(swipeController));
//        }

//        private void SwipeControllerActions_LeftButtonEvent(object sender, int position)
//        {
//            if (position < 0)
//                return;

//            var item = swipeableListView.Items.ToList()[position];
//            swipeableListView.LeftButtonCommand.Execute(item);
//        }

//        private void SwipeControllerActions_RightButtonEvent(object sender, int position)
//        {
//            if (position < 0)
//                return;

//            var item = swipeableListView.Items.ToList()[position];
//            swipeableListView.RightButtonCommand.Execute(item);
//        }

//        private void SetContextMenuButtons()
//        {
//            if (swipeableListView == null)
//                return;

//            swipeController.LeftButtonEnabled = swipeableListView.LeftButtonEnabled;
//            swipeController.LeftButtonColor = droid.Graphics.Color.ParseColor(swipeableListView.LeftButtonColor);

//            swipeController.RightButtonEnabled = swipeableListView.RightButtonEnabled;
//            swipeController.RightButtonColor = droid.Graphics.Color.ParseColor(swipeableListView.RightButtonColor);
//        }

//        private void SetAdapter(IEnumerable<Swipeable> items)
//        {
//            if (items == null)
//                return;

//            // Prepare the data source:
//            // Instantiate the adapter and pass in its data source:
//            adapter = new ItemAdapter(items, swipeController, swipeableListView);

//            // Plug the adapter into the RecyclerView:
//            recyclerView.SetAdapter(adapter);
//        }

//        protected override void OnElementChanged(ElementChangedEventArgs<SwipeableListView> e)
//        {
//            base.OnElementChanged(e);

//            if (e.OldElement != null)
//            {
//                // Unsubscribe
//                SwipeableListView.ItemsChangedEvent -= SwipeableListView_ItemsChangedEvent;
//            }

//            if (e.NewElement != null)
//            {
//                swipeableListView = e.NewElement;

//                SwipeableListView.ItemsChangedEvent += SwipeableListView_ItemsChangedEvent;

//                if (Control == null)
//                {
//                    var linearLayout = new LinearLayout(Context);
//                    linearLayout.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);

//                    // Plug in the linear layout manager:
//                    var mLayoutManager = new LinearLayoutManager(Context);
//                    recyclerView.SetLayoutManager(mLayoutManager);

//                    linearLayout.AddView(recyclerView);
//                    SetNativeControl(linearLayout);
//                }
//            }

//            SetContextMenuButtons();

//            SetAdapter(e.NewElement.Items);
//        }

//        private void SwipeableListView_ItemsChangedEvent(object sender, IEnumerable<Swipeable> e)
//        {
//            swipeableListView.Items = e;
//            SetAdapter(e);
//            SetContextMenuButtons();
//        }
//    }

//    public class ItemViewHolder : RecyclerView.ViewHolder
//    {
//        private droid.Graphics.Color defaultColor = droid.Graphics.Color.ParseColor("#007ac9");

//        public TextView Title { get; private set; }

//        public TextView Message { get; private set; }

//        public droid.Widget.Button NewInfoIndicatorButton { get; private set; }

//        public TextView Date { get; private set; }

//        public ItemViewHolder(droid.Views.View itemView) : base(itemView)
//        {
//            // Locate and cache view references:
//            Title = itemView.FindViewById<TextView>(MAUI.Clinical6.Resource.Id.text_title);
//            Title.SetTextColor(defaultColor);

//            Message = itemView.FindViewById<TextView>(MAUI.Clinical6.Resource.Id.text_message);
//            Message.SetTextColor(droid.Graphics.Color.Black);
//            Message.SetMaxLines(1);

//            Date = itemView.FindViewById<TextView>(MAUI.Clinical6.Resource.Id.text_date);
//            NewInfoIndicatorButton = itemView.FindViewById<droid.Widget.Button>(MAUI.Clinical6.Resource.Id.button_red_circle);
//        }
//    }

//public class ItemAdapter : RecyclerView.Adapter
//{
//    private SwipeController swipeController;

//    public List<Swipeable> Items { get; }

//    private readonly SwipeableListView swipeableListView;

//    public ItemAdapter(IEnumerable<Swipeable> items,
//        SwipeController swipeController, SwipeableListView swipeableListView)
//    {
//        this.Items = new List<Swipeable>(items);
//        this.swipeController = swipeController;
//        this.swipeableListView = swipeableListView;
//    }

//    public override int ItemCount
//    {
//        get
//        {
//            if (Items == null)
//                return 0;

//            return Items.ToList().Count();
//        }
//    }

//    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
//    {
//        ItemViewHolder viewHolder = holder as ItemViewHolder;
//        var swipeable = Items[position];

//        // Load the photo caption from the photo album:
//        viewHolder.Title.Text = Items.ToList()[position].Title;
//        viewHolder.Message.Text = Items.ToList()[position].Description;
//        viewHolder.Date.Text = swipeable.ExtraInfo;

//        if (!swipeable.IsNewInfoAvailable)
//            viewHolder.NewInfoIndicatorButton.Visibility = ViewStates.Invisible;

//        viewHolder.ItemView.Click += (object sender, EventArgs e) =>
//        {
//            if (swipeableListView.SelectedItemCommand == null)
//                return;

//            if (swipeController.CurrentButtonState != ButtonState.GONE)
//                return;

//            swipeableListView.SelectedItemCommand.Execute(Items[position]);
//        };
//    }

//    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
//    {
//        // Inflate the CardView for the photo:
//        droid.Views.View itemView = LayoutInflater.From(parent.Context).Inflate(MAUI.Clinical6.Resource.Layout.CustomListViewRendererCell, parent, false);

//        // Create a ViewHolder to hold view references inside the CardView:
//        ItemViewHolder vh = new ItemViewHolder(itemView);
//        return vh;
//    }
//}
//}

using System;
using System.IO;
using Android.Views;
using Android.Widget;
using Com.Opentok.Android;
using Droid = global::Android;

namespace Xamarin.Forms.Clinical6.Android.Services
{
    /// <summary>
    /// Open tok streaming service.
    /// </summary>
    public class OpenTokStreamingService
    {
        #region private state

        protected object _syncRoot = new object();
        protected string _apiKey;
        protected string _sessionId;
        protected string _userToken;
        protected bool _subscriberConnected;
        protected bool _sessionTerminationIsInProgress = false;

        #endregion

        /// <summary>
        /// Gets the android context.
        /// </summary>
        /// <value>The android context.</value>
        private global::Android.Content.Context AndroidContext
        {
            get
            {
                return global::Android.App.Application.Context;
            }
        }

        /// <summary>
        /// Occurs when on session ended.
        /// </summary>
        public event Action OnSessionEnded = delegate { };
        /// <summary>
        /// Occurs when on publish started.
        /// </summary>
        public event Action OnPublishStarted = delegate { };

        #region singletone

        private static OpenTokStreamingService _instance = new OpenTokStreamingService();

        public static OpenTokStreamingService Instance
        {
            get { return _instance; }
        }

        #endregion

        #region private state

        private Session _session;
        private Publisher _publisher;
        private Subscriber _subscriber;
        private ViewGroup _myStreamContainer;
        private ViewGroup _otherStreamContainer;

        #endregion

        private bool _isVideoPublishingEnabled;

        public bool IsVideoPublishingEnabled
        {
            get { return _isVideoPublishingEnabled; }
            set
            {
                if (_isVideoPublishingEnabled == value)
                    return;
                _isVideoPublishingEnabled = value;
                OnVideoPublishingChanged();
            }
        }

        private bool _isAudioPublishingEnabled;

        public bool IsAudioPublishingEnabled
        {
            get
            {
                return _isAudioPublishingEnabled;
            }
            set
            {
                if (_isAudioPublishingEnabled == value)
                    return;
                _isAudioPublishingEnabled = value;
                OnAudioPublishingChanged();
            }
        }

        private bool _isVideoSubscriptionEnabled;

        public bool IsVideoSubscriptionEnabled
        {
            get { return _isVideoSubscriptionEnabled; }
            set
            {
                if (_isVideoSubscriptionEnabled == value)
                    return;
                _isVideoSubscriptionEnabled = value;
                OnVideoSubscriptionChanged();
            }
        }

        private bool _isAudioSubscriptionEnabled;

        public bool IsAudioSubscriptionEnabled
        {
            get
            {
                return _isAudioSubscriptionEnabled;
            }
            set
            {
                if (_isAudioSubscriptionEnabled == value)
                    return;
                _isAudioSubscriptionEnabled = value;
                OnAudioSubscriptionChanged();
            }
        }

        private bool _isSubscriberVideoEnabled;

        public bool IsSubscriberVideoEnabled
        {
            get { return _isSubscriberVideoEnabled; }
            protected set
            {
                if (_isSubscriberVideoEnabled == value)
                    return;
                _isSubscriberVideoEnabled = value;
            }
        }

        public void Dispose()
        {
            EndSession();
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Xamarin.Forms.Clinical6.Android.Services.OpenTokStreamingService"/> class.
        /// </summary>
        private OpenTokStreamingService()
        {
        }

        /// <summary>
        /// Inits the new session.
        /// </summary>
        /// <param name="apiKey">API key.</param>
        /// <param name="sessionId">Session identifier.</param>
        /// <param name="userToken">User token.</param>
        public void InitNewSession(string apiKey, string sessionId, string userToken)
        {
            if (_session != null)
            {
                //stop running session if any and dispose all the resources
                EndSession();
            }

            _apiKey = apiKey;
            _sessionId = sessionId;
            _userToken = userToken;

            IsVideoPublishingEnabled = true;
            IsAudioPublishingEnabled = true;
            IsVideoSubscriptionEnabled = true;
            IsAudioSubscriptionEnabled = true;
            IsSubscriberVideoEnabled = true;

            _session = new Session(AndroidContext, _apiKey, _sessionId);
            SubscribeForSessionEvents(_session);

            _session.Connect(_userToken);
        }

        /// <summary>
        /// Publish this instance.
        /// </summary>
        private void Publish()
        {
            lock (_syncRoot)
            {
                if (_publisher != null || _session == null)
                    return;

                _publisher = new Publisher(AndroidContext, Environment.TickCount.ToString());
                //_publisher = new Publisher(Android.App.Application.Context, Environment.TickCount.ToString());

                SubscribeForPublsherEvents(_publisher, true);
                _publisher.SetStyle(BaseVideoRenderer.StyleVideoScale, BaseVideoRenderer.StyleVideoFill);
                if (_publisher.PublishAudio != IsAudioPublishingEnabled)
                    _publisher.PublishAudio = IsAudioPublishingEnabled;
                if (_publisher.PublishVideo != IsVideoPublishingEnabled)
                    _publisher.PublishVideo = IsVideoPublishingEnabled;
                _session.Publish(_publisher);

                ActivateStreamContainer(_myStreamContainer, _publisher.View);

                //opentok issue, after session start to publish audio/video, PublishVideo property changed it's value to true, need to revert it to IsVideoPublishingEnabled value
                if (_publisher.PublishVideo != IsVideoPublishingEnabled)
                    _publisher.PublishVideo = IsVideoPublishingEnabled;
            }
        }

        /// <summary>
        /// Subscribe the specified stream.
        /// </summary>
        /// <param name="stream">Stream.</param>
        private void Subscribe(Com.Opentok.Android.Stream stream)
        {
            lock (_syncRoot)
            {
                if (_subscriber != null || _session == null)
                    return;


                 _subscriber = new Subscriber(global::Android.App.Application.Context, stream);

                SubscribeForSubscriberEvents(_subscriber);
                _subscriber.SetStyle(BaseVideoRenderer.StyleVideoScale, BaseVideoRenderer.StyleVideoFill);
                if (_subscriber.SubscribeToAudio != IsAudioSubscriptionEnabled)
                    _subscriber.SubscribeToAudio = IsAudioSubscriptionEnabled;
                if (_subscriber.SubscribeToVideo != IsVideoSubscriptionEnabled)
                    _subscriber.SubscribeToVideo = IsVideoSubscriptionEnabled;
                _session.Subscribe(_subscriber);
            }
        }

        /// <summary>
        /// Ends the session.
        /// </summary>
        public void EndSession()
        {
            lock (_syncRoot)
            {
                try
                {
                    _sessionTerminationIsInProgress = true;
                    if (_subscriber != null)
                    {
                        if (_subscriber.SubscribeToAudio)
                            _subscriber.SubscribeToAudio = false;
                        if (_subscriber.SubscribeToVideo)
                            _subscriber.SubscribeToVideo = false;
                        SubscribeForSubscriberEvents(_subscriber, false);
                        _subscriber.Dispose();
                        _subscriber = null;
                    }

                    if (_publisher != null)
                    {
                        if (_publisher.PublishAudio)
                            _publisher.PublishAudio = false;
                        if (_publisher.PublishVideo)
                            _publisher.PublishVideo = false;
                        SubscribeForPublsherEvents(_publisher, false);
                        _publisher.Dispose();
                        _publisher = null;
                    }

                    if (_session != null)
                    {
                        SubscribeForSessionEvents(_session, false);
                        _session.Disconnect();
                        _session.Dispose();
                        _session = null;
                    }


                    DeactivateStreamContainer(_myStreamContainer);
                    _myStreamContainer = null;

                    DeactivateStreamContainer(_otherStreamContainer);
                    _otherStreamContainer = null;


                    _apiKey = null;
                    _sessionId = null;
                    _userToken = null;

                    IsVideoPublishingEnabled = false;
                    IsAudioPublishingEnabled = false;
                    IsVideoSubscriptionEnabled = false;
                    IsAudioSubscriptionEnabled = false;
                    IsSubscriberVideoEnabled = false;

                    _subscriberConnected = false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    _sessionTerminationIsInProgress = false;
                    OnSessionEnded();
                }
            }
        }

        /// <summary>
        /// Swaps the camera.
        /// </summary>
        public void SwapCamera()
        {
            if (_publisher == null)
                return;

            _publisher.CycleCamera();
        }

        /// <summary>
        /// Ons the video publishing changed.
        /// </summary>
        protected void OnVideoPublishingChanged()
        {
            if (_publisher == null)
                return;
            if (_publisher.PublishVideo != IsVideoPublishingEnabled)
                _publisher.PublishVideo = IsVideoPublishingEnabled;
        }

        /// <summary>
        /// Ons the audio publishing changed.
        /// </summary>
        protected void OnAudioPublishingChanged()
        {
            if (_publisher == null)
                return;
            if (_publisher.PublishAudio != IsAudioPublishingEnabled)
                _publisher.PublishAudio = IsAudioPublishingEnabled;
        }

        /// <summary>
        /// Ons the video subscription changed.
        /// </summary>
        protected void OnVideoSubscriptionChanged()
        {
            if (_subscriber == null)
                return;
            if (_subscriber.SubscribeToVideo != IsVideoSubscriptionEnabled)
                _subscriber.SubscribeToVideo = IsVideoSubscriptionEnabled;
        }

        /// <summary>
        /// Ons the audio subscription changed.
        /// </summary>
        protected void OnAudioSubscriptionChanged()
        {
            if (_subscriber == null)
                return;
            if (_subscriber.SubscribeToAudio != IsAudioSubscriptionEnabled)
                _subscriber.SubscribeToAudio = IsAudioSubscriptionEnabled;
        }

        /// <summary>
        /// Sets my stream container.
        /// </summary>
        /// <param name="container">Container.</param>
        public void SetMyStreamContainer(object container)
        {
            _myStreamContainer = ((ViewGroup)container);
        }

        /// <summary>
        /// Sets the other stream container.
        /// </summary>
        /// <param name="container">Container.</param>
        public void SetOtherStreamContainer(object container)
        {
            _otherStreamContainer = ((ViewGroup)container);
        }

        /// <summary>
        /// Sets the stream container.
        /// </summary>
        /// <param name="container">Container.</param>
        /// <param name="myStream">If set to <c>true</c> my stream.</param>
        public void SetStreamContainer(object container, bool myStream)
        {
            var streamContainer = ((ViewGroup)container);
            global::Android.Views.View streamView = null;

            if (myStream)
            {
                _myStreamContainer = streamContainer;
                if (_publisher != null)
                    streamView = _publisher.View;
            }
            else
            {
                _otherStreamContainer = streamContainer;
                if (_subscriber != null)
                    streamView = _subscriber.View;
            }

            ActivateStreamContainer(streamContainer, streamView);
        }

        #region private helpers
        /// <summary>
        /// Activates the stream container.
        /// </summary>
        /// <param name="streamContainer">Stream container.</param>
        /// <param name="streamView">Stream view.</param>
        private void ActivateStreamContainer(ViewGroup streamContainer, global::Android.Views.View streamView)
        {
            DeactivateStreamContainer(streamContainer);
            if (streamContainer == null || streamView == null)
                return;

            if (streamView.Parent != null)
                ((ViewGroup)streamView.Parent).RemoveView(streamView);

            var layoutParams = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            streamContainer.AddView(streamView, layoutParams);
        }

        /// <summary>
        /// Deactivates the stream container.
        /// </summary>
        /// <param name="streamContainer">Stream container.</param>
        private void DeactivateStreamContainer(ViewGroup streamContainer)
        {
            if (streamContainer == null)
                return;

            streamContainer.RemoveAllViews();
            streamContainer.SetBackgroundColor(global::Android.Graphics.Color.Transparent);
        }

        #endregion

        #region events subscription
        /// <summary>
        /// Subscribes for session events.
        /// </summary>
        /// <param name="session">Session.</param>
        /// <param name="subscribe">If set to <c>true</c> subscribe.</param>
        private void SubscribeForSessionEvents(Session session, bool subscribe = true)
        {
            if (session == null)
                return;

            if (subscribe)
            {
                session.ConnectionDestroyed += OnConnectionDestroyed;
                session.Connected += OnDidConnect;
                session.StreamReceived += OnStreamCreated;
                session.StreamDropped += OnStreamDestroyed;
            }
            else
            {
                session.ConnectionDestroyed -= OnConnectionDestroyed;
                session.Connected -= OnDidConnect;
                session.StreamReceived -= OnStreamCreated;
                session.StreamDropped -= OnStreamDestroyed;
            }
        }

        /// <summary>
        /// Subscribes for subscriber events.
        /// </summary>
        /// <param name="subscriber">Subscriber.</param>
        /// <param name="subscribe">If set to <c>true</c> subscribe.</param>
        private void SubscribeForSubscriberEvents(Subscriber subscriber, bool subscribe = true)
        {
            if (subscriber == null)
                return;

            if (subscribe)
            {
                subscriber.Connected += OnSubscriberDidConnectToStream;
                subscriber.VideoDisabled += OnSubscriberVideoDisabled;
                subscriber.VideoEnabled += OnSubscriberVideoEnabled;
            }
            else
            {
                subscriber.Connected -= OnSubscriberDidConnectToStream;
                subscriber.VideoDisabled -= OnSubscriberVideoDisabled;
                subscriber.VideoEnabled -= OnSubscriberVideoEnabled;
            }
        }

        /// <summary>
        /// Subscribes for publsher events.
        /// </summary>
        /// <param name="publisher">Publisher.</param>
        /// <param name="subscribe">If set to <c>true</c> subscribe.</param>
        private void SubscribeForPublsherEvents(Publisher publisher, bool subscribe = true)
        {
            if (publisher == null)
                return;

            if (subscribe)
            {
                publisher.StreamCreated += OnPublisherStreamCreated;
            }
            else
            {
                publisher.StreamCreated -= OnPublisherStreamCreated;
            }
        }


        #endregion

        #region session events
        /// <summary>
        /// Ons the connection destroyed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OnConnectionDestroyed(object sender, Session.ConnectionDestroyedEventArgs e)
        {
            EndSession();
        }

        /// <summary>
        /// Ons the did connect.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OnDidConnect(object sender, EventArgs e)
        {
            Publish();
        }

        /// <summary>
        /// Ons the stream created.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OnStreamCreated(object sender, Session.StreamReceivedEventArgs e)
        {
            Subscribe(e.P1);
        }

        /// <summary>
        /// Ons the stream destroyed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OnStreamDestroyed(object sender, Session.StreamDroppedEventArgs e)
        {
            DeactivateStreamContainer(_myStreamContainer);
            DeactivateStreamContainer(_otherStreamContainer);
        }

        #endregion

        #region subscriber evenets
        /// <summary>
        /// Ons the subscriber video disabled.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OnSubscriberVideoDisabled(object sender, Subscriber.VideoDisabledEventArgs e)
        {
            IsSubscriberVideoEnabled = false;
        }

        /// <summary>
        /// Ons the subscriber video enabled.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OnSubscriberVideoEnabled(object sender, Subscriber.VideoEnabledEventArgs e)
        {
            lock (_syncRoot)
            {
                if (_subscriber != null && _subscriber.Stream != null && _subscriber.Stream.HasVideo)
                    IsSubscriberVideoEnabled = true;
            }
        }

        /// <summary>
        /// Ons the subscriber did connect to stream.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void OnSubscriberDidConnectToStream(object sender, EventArgs e)
        {
            lock (_syncRoot)
            {
                if (_subscriber != null)
                {
                    _subscriberConnected = true;
                    ActivateStreamContainer(_otherStreamContainer, _subscriber.View);
                    if (_subscriber.Stream != null && _subscriber.Stream.HasVideo)
                        IsSubscriberVideoEnabled = true;
                }
            }
        }

        #endregion

        #region publisher evenets
        /// <summary>
        /// Ons the publisher stream created.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void OnPublisherStreamCreated(object sender, PublisherKit.StreamCreatedEventArgs e)
        {
            OnPublishStarted();
        }

        #endregion
    }
}

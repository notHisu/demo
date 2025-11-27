using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Xamarin.Forms.Clinical6.Core.Helpers
{
    /// <summary>
    /// Supports basic reporting on whether an action is busy
    /// </summary>
    /// <remarks>
    /// This class is not typically used directly.  See the extensions, most
    /// notably ActionTracker.
    /// </remarks>
    public class BusyTracker : ObservableObject
    {
        protected BehaviorSubject<bool> IsBusySubject = new BehaviorSubject<bool>(false);
        public IObservable<bool> IsBusyChanges { get; protected set; }

        private bool _isBusy;

        public virtual bool IsBusy
        {
            get => _isBusy;
            protected set
            {
                SetProperty(ref _isBusy, value);
                IsBusySubject.OnNext(value);
            }
        }

        public BusyTracker()
        {
            IsBusyChanges = IsBusySubject.Publish().RefCount();
        }
    }

    /// <summary>
    /// Aggregates the "IsBusy" status of multiple other busy trackers
    /// </summary>
    /// <remarks>IsBusy will only be false if ALL child trackers are not busy.</remarks>
    public class MultiBusyTracker : BusyTracker
    {
        public MultiBusyTracker(params BusyTracker[] trackers) : this(trackers as IEnumerable<BusyTracker>)
        {
        }

        public MultiBusyTracker(IEnumerable<BusyTracker> trackers)
        {
            // The overall tracker is busy if any of its children are busy
            IsBusyChanges = Observable.CombineLatest(trackers.Select(t => t.IsBusyChanges))
                .Select(busyValues => busyValues.Any(b => b))
                .Publish()
                .RefCount();

            IsBusyChanges.Subscribe(isBusy => IsBusy = isBusy);
        }
    }

    /// <summary>
    /// Wraps an asynchronous action in busy tracking and
    /// basic error handling
    /// </summary>
    /// <remarks>
    /// This is commonly used as a replacement/enhancement for Commands in the ViewModel.
    /// Instead of creating a new Command, Create a new ActionTracker and specify the
    /// action and - optionally - the error handling function.  You can then
    /// bind any UI elements to the tracker's Command property to get the usual
    /// behavior.
    /// 
    /// This differs from a vanilla command in the following ways:
    /// - Exposes a bindable IsBusy bool to indicate that the command is busy executing
    /// - Prevents simultaneous execution: if the command is busy, it cannot be run
    ///   again until it completes.
    /// - Defines optional error handling.
    /// </remarks>
    public class ActionTracker<TInput> : BusyTracker
    {
        protected readonly Func<TInput, Task> Execute;
        protected readonly Func<Exception, string> GetErrorMessage;
        protected readonly Func<Exception, Task> OnError;

        public Command<TInput> Command { get; }

        public override bool IsBusy
        {
            get => base.IsBusy;
            protected set
            {
                base.IsBusy = value;
                Command.ChangeCanExecute();
            }
        }

        /// <summary>
        /// Sets the is busy.
        /// </summary>
        public void SetIsBusy()
        {
            base.IsBusy = true;
        }

        private string _errorImage;
        public string ErrorImage
        {
            get => _errorImage;
            set => SetProperty(ref _errorImage, value);
        }

        private string _errorTitle;
        public string ErrorTitle
        {
            get => _errorTitle;
            set => SetProperty(ref _errorTitle, value);
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        /// <summary>
        /// Create an action tracker
        /// </summary>
        /// <param name="execute">The logic to run on execution</param>
        /// <param name="canExecute">Optional custom CanExecute behavior.  Note that, in order for the Command to update, you will need to invoke ChangeCanExecute from outside of this class</param>
        /// <param name="onError">Optional logic to run if the action fails</param>
        /// <param name="getErrorMessage">Optional logic to determine the user-facing error message on failure</param>
        public ActionTracker(Func<TInput, Task> execute, Func<TInput, bool> canExecute = null, Func<Exception, Task> onError = null, Func<Exception, string> getErrorMessage = null)
        {
            Execute = execute;
            OnError = onError;
            GetErrorMessage = getErrorMessage ?? GetDefaultErrorMessage;

            Command = MakeCommand(canExecute);
            Command.ChangeCanExecute();
        }

        public async Task Run(TInput input)
        {
            // Disallow simultaneous requests
            if (IsBusy) return;

            try
            {
                ErrorImage = null;
                ErrorTitle = null;
                ErrorMessage = null;
                IsBusy = true;
                await Execute(input);
            }
            catch (Exception ex)
            {
                ErrorMessage = GetErrorMessage(ex);
                if (OnError == null) throw;
                await OnError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Wrap the execution in a Command for exposure in a ViewModel
        /// </summary>
        protected Command<TInput> MakeCommand(Func<TInput, bool> canExecute)
        {
            if (canExecute == null) canExecute = _ => true;

            return new Command<TInput>(
                async input => await Run(input),
                input => !IsBusy && canExecute(input)
            );
        }

        public static Task IgnoreErrors(Exception ex) => Task.FromResult(false);

        /// <summary>
        /// The default behavior for errors is to set a friendly error
        /// </summary>
        public static string GetDefaultErrorMessage(Exception ex)
        {
            string errorStr = ex.ToString();
            Console.WriteLine(errorStr);
            string exceptionStr = "ErrorMessageFriendly".Localized();
#if DEBUG
            // Add additonal exception info.
            exceptionStr += "\nError:" + errorStr;
#endif
            return exceptionStr;
        }
    }

    /// <summary>
    /// Variation on the standard ActionTracker with no meaningful input
    /// </summary>
    public class ActionTracker : ActionTracker<object>
    {
        public ActionTracker(Func<Task> execute, Func<bool> canExecute = null, Func<Exception, Task> onError = null, Func<Exception, string> getErrorMessage = null)
            : base(ConvertAction(execute), ConvertAction(canExecute), onError, getErrorMessage)
        {
        }

        public async Task Run()
        {
            await Run(null);
        }

        private static Func<object, T> ConvertAction<T>(Func<T> action)
        {
            if (action == null) return null;
            return _ => action();
        }
    }
}
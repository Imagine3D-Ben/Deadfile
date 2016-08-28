using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Threading;

namespace ObservableImmutable
{
    public abstract class ObservableCollectionObject : INotifyCollectionChanged, INotifyPropertyChanged
    {
        #region Private

        private readonly IChubb chubb;
        private IDispatcher dispatcher;

        #endregion Private

        #region Public Properties

        private readonly LockTypeEnum _lockType;
        public LockTypeEnum LockType
        {
            get
            {
                return _lockType;
            }
        }

        #endregion Public Properties

        #region Constructor

        protected ObservableCollectionObject(LockTypeEnum lockType, IDispatcher dispatcher, IChubbFactory chubbFactory)
        {
            _lockType = lockType;
            chubb = chubbFactory.CreateChubb();
            this.dispatcher = dispatcher;
        }

        #endregion Constructor

        #region SpinWait/PumpWait Methods

        // note : find time to put all these methods into a helper class instead of in a base class

        // returns a valid dispatcher if this is a UI thread (can be more than one UI thread so different dispatchers are possible); null if not a UI thread
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected IDispatcher GetDispatcher()
        {
            //            return Dispatcher.FromThread(Thread.CurrentThread);
            return this.dispatcher;
        }

        protected void WaitForCondition(Func<bool> condition)
        {
            var dispatcher = GetDispatcher();

            chubb.WaitForCondition(LockType, condition);
        }

        protected void PumpWait_PumpUntil(IDispatcher dispatcher, Func<bool> condition)
        {
            var frame = new DispatcherFrame();
            BeginInvokePump(dispatcher, frame, condition);
            dispatcher.PushFrame(frame);
        }

        private static void BeginInvokePump(IDispatcher dispatcher, DispatcherFrame frame, Func<bool> condition)
        {
            dispatcher.BeginInvoke
                (
                DispatcherPriority.DataBind,
                (Action)
                    (
                    () =>
                        {
                            frame.Continue = !condition();
                            if (frame.Continue)
                                BeginInvokePump(dispatcher, frame, condition);
                        }
                    )
                );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DoEvents()
        {
            var dispatcher = GetDispatcher();
            if (dispatcher == null)
            {
                return;
            }

            var frame = new DispatcherFrame();
            dispatcher.BeginInvoke(DispatcherPriority.DataBind, new DispatcherOperationCallback(ExitFrame), frame);
            dispatcher.PushFrame(frame);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static object ExitFrame(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool TryLock()
        {
            return chubb.TryLock(LockType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Lock()
        {
            chubb.Lock(LockType);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Unlock()
        {
            chubb.Unlock(LockType);
        }

        #endregion SpinWait/PumpWait Methods

        #region INotifyCollectionChanged

        public virtual event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            var notifyCollectionChangedEventHandler = CollectionChanged;

            if (notifyCollectionChangedEventHandler == null)
                return;

            foreach (NotifyCollectionChangedEventHandler handler in notifyCollectionChangedEventHandler.GetInvocationList())
            {
                dispatcher.Invoke(DispatcherPriority.DataBind, (a, b) => handler(a, (NotifyCollectionChangedEventArgs)b), this, args);
            }
        }

        protected virtual void RaiseNotifyCollectionChanged()
        {
            RaiseNotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected virtual void RaiseNotifyCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            RaisePropertyChanged("Count");
            RaisePropertyChanged("Item[]");
            OnCollectionChanged(args);
        }

        #endregion INotifyCollectionChanged

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            var propertyChangedEventHandler = PropertyChanged;

            if (propertyChangedEventHandler != null)
            {
                propertyChangedEventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged

    }
}

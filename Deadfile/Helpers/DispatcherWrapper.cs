using ObservableImmutable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Practices.Prism.PubSubEvents;

namespace Deadfile.Helpers
{
    public class DispatcherWrapper : IDispatcher
    {
        static object syncLock = new object();
        static DispatcherWrapper instance;
        Dispatcher dispatcher;

        public static IDispatcher Instance
        {
            get
            {
                lock (syncLock)
                {
                    if (instance == null)
                    {
                        instance = new DispatcherWrapper();
                    }
                }
                return instance;
            }
        }

        public DispatcherWrapper()
        {
            if (Application.Current != null)
            {
                dispatcher = Application.Current.Dispatcher;
            }
            else
            {
                //this is useful for unit tests where there is no application running 
                dispatcher = Dispatcher.CurrentDispatcher;
            }
        }
        public void Invoke(Action action)
        {
            dispatcher.Invoke(action);
        }

        public void BeginInvoke(Action action)
        {
            dispatcher.BeginInvoke(action);
        }

        public void BeginInvoke(DispatcherPriority priority, DispatcherOperationCallback callback, DispatcherFrame frame)
        {
            dispatcher.BeginInvoke(priority, callback, frame);
        }

        public void PushFrame(DispatcherFrame frame)
        {
            Dispatcher.PushFrame(frame);
        }

        public void BeginInvoke(DispatcherPriority priority, Action callback)
        {
            dispatcher.BeginInvoke(priority, callback);
        }

        public void Invoke(DispatcherPriority priority, Action<object, object> callback, object invoker, object args)
        {
            var dispatcherObject = callback.Target as DispatcherObject;

            if (dispatcherObject != null && !dispatcherObject.CheckAccess())
            {
                dispatcherObject.Dispatcher.Invoke(priority, callback, invoker, args);
            }
            else
                callback(invoker, args);
        }

        public ThreadOption BackgroundThread()
        {
            return ThreadOption.BackgroundThread;
        }

        public ThreadOption UIThread()
        {
            return ThreadOption.UIThread;
        }
    }
}

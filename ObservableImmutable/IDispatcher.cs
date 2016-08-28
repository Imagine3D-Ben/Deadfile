using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ObservableImmutable
{
    public interface IDispatcher
    {
        void Invoke(Action action);

        void BeginInvoke(Action action);

        void BeginInvoke(DispatcherPriority priority, DispatcherOperationCallback callback, DispatcherFrame frame);

        void Invoke(DispatcherPriority priority, Action<object, object> callback, object invoker, object args);

        void BeginInvoke(DispatcherPriority priority, Action callback);

        void PushFrame(DispatcherFrame frame);

        ThreadOption BackgroundThread();

        ThreadOption UIThread();
    }
}

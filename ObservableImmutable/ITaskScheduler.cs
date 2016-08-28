using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableImmutable
{
    public interface ITaskScheduler
    {
        Task Run(Action action);
    }

    public sealed class TaskSchedulerImpl : ITaskScheduler
    {
        public Task Run(Action action)
        {
            return Task.Run(action);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObservableImmutable
{
    public enum LockTypeEnum
    {
        SpinWait,
        Lock
    }

    public interface IChub
    {
        void WaitForCondition(LockTypeEnum lockType, Func<bool> condition);
        bool TryLock(LockTypeEnum lockType);
        void Lock(LockTypeEnum lockType);
        void Unlock(LockTypeEnum lockType);
    }

    public interface ILock
    {
        IChub CreateChub();
    }
}

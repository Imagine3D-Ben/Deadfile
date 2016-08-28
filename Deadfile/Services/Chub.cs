using ObservableImmutable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Deadfile.Services
{
    public sealed class ChubFactory : ILock
    {
        public IChub CreateChub()
        {
            return new Chub();
        }
    }

    public sealed class Chub : IChub
    {
        private bool _lockObjWasTaken;
        private readonly object _lockObj;
        private int _lock; // 0=unlocked        1=locked

        public Chub()
        {
            _lockObj = new object();
        }

        public void WaitForCondition(LockTypeEnum lockType, Func<bool> condition)
        {
            switch (lockType)
            {
                case LockTypeEnum.SpinWait:
                    SpinWait.SpinUntil(condition); // spin baby... 
                    break;
                case LockTypeEnum.Lock:
                    var isLockTaken = false;
                    Monitor.Enter(_lockObj, ref isLockTaken);
                    _lockObjWasTaken = isLockTaken;
                    break;
            }
            return;
        }

        public bool TryLock(LockTypeEnum lockType)
        {
            switch (lockType)
            {
                case LockTypeEnum.SpinWait:
                    return Interlocked.CompareExchange(ref _lock, 1, 0) == 0;
                case LockTypeEnum.Lock:
                    return Monitor.TryEnter(_lockObj);
            }

            return false;
        }

        public void Lock(LockTypeEnum lockType)
        {
            switch (lockType)
            {
                case LockTypeEnum.SpinWait:
                    WaitForCondition(lockType, () => Interlocked.CompareExchange(ref _lock, 1, 0) == 0);
                    break;
                case LockTypeEnum.Lock:
                    WaitForCondition(lockType, () => Monitor.TryEnter(_lockObj));
                    break;
            }
        }

        public void Unlock(LockTypeEnum lockType)
        {
            switch (lockType)
            {
                case LockTypeEnum.SpinWait:
                    _lock = 0;
                    break;
                case LockTypeEnum.Lock:
                    if (_lockObjWasTaken)
                        Monitor.Exit(_lockObj);
                    _lockObjWasTaken = false;
                    break;
            }
        }
    }
}

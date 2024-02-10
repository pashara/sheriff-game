using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Sheriff.InputLock
{
    public static class TouchLocker
    {
        private class LockHandling : IDisposable
        {
            private readonly HashSet<LockHandling> _handlers;
            private readonly Action _check;

            public LockHandling(HashSet<LockHandling> handlers, Action check)
            {
                _handlers = handlers;
                _check = check;
                _handlers.Add(this);
                _check?.Invoke();
            }

            public void Dispose()
            {
                _handlers.Remove(this);
                _check?.Invoke();
            }
        }
        
        private static readonly HashSet<LockHandling> LockHandlers = new();
        private static readonly ReactiveProperty<bool> Locked = new();
        
        public static IReadOnlyReactiveProperty<bool> IsLocked => Locked;
        
        
        public static IDisposable Lock()
        {
            return new LockHandling(LockHandlers, Check);
        }

        public static void UnlockAll()
        {
            foreach (var handling in LockHandlers.ToList())
            {
                handling.Dispose();
            }
        }

        private static void Check()
        {
            Locked.Value = LockHandlers.Count == 0;
        }
    }
}
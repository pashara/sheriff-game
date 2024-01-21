using System;
using System.Collections.Generic;

namespace Sheriff.GameFlow
{
    public class ActionHandleService
    {
        private Dictionary<Type, object> _handlers = new();
        public void Put<T2, T3>(BaseAction<T2, T3> handler) where T2 : ActionParam where T3 : EmulateActionParams
        {
            _handlers[handler.GetType()] = handler;
        }
        
        public void Emulate<T1, T2, T3>(T3 emulateParams) where T1 : BaseAction<T2, T3> where T2 : ActionParam where T3 : EmulateActionParams
        {
            if (_handlers.TryGetValue(typeof(T1), out var handler) && handler is T1 h)
            {
                h.Emulate(emulateParams);
            }

            return;
        }
        
        public void Emulate<T2, T3>(Type type, T3 emulateParams) where T2 : ActionParam where T3 : EmulateActionParams
        {
            if (_handlers.TryGetValue(type, out var handler) && handler is BaseAction<T2, T3> h)
            {
                h.Emulate(emulateParams);
            }

            return;
        }
        
        public T3 Simulate<T1, T2, T3>(T2 inParams) where T1 : BaseAction<T2, T3> where T2 : ActionParam where T3 : EmulateActionParams
        {
            if (_handlers.TryGetValue(typeof(T1), out var handler) && handler is T1 h)
            {
                return h.Simulate(inParams);
            }

            return default;
        }
        
        public T3 Simulate<T2, T3>(Type type, T2 inParams)where T2 : ActionParam where T3 : EmulateActionParams
        {
            if (_handlers.TryGetValue(type, out var handler1) && handler1 is BaseAction<T2, T3> h1)
            {
                h1.Simulate(inParams);
            }

            return default;
        }
    }
}
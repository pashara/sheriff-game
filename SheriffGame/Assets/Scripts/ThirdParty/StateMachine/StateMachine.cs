using System;
using System.Collections.Generic;
using ThirdParty.StateMachine.States;
using UniRx;
using UnityEngine;

namespace ThirdParty.StateMachine
{
    public abstract class StateMachine<T> : IStateMachine<T> where T : IState
    {
        private ReactiveProperty<T> _actualState = new();
        protected Dictionary<Type, T> States { get; } = new();
        public IReadOnlyReactiveProperty<T> ActualState => _actualState;

        public void Enter<TState>() where TState : T
        {
            if (ActualState != null && ActualState.GetType() == typeof(TState))
            {
                return;
            }
            
            ExitActualState();

            if (!TryGetHandler<TState>(out var stateHandler))
                return;
            
            ProcessEnterState(stateHandler);
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : T, IPayloadableState<TPayload> where TPayload : IStatePayload
        {
            ExitActualState();
            
            if (!TryGetHandler<TState>(out var stateHandler))
                return;

            var handler = stateHandler as IPayloadableState<TPayload>;
            handler?.Configure(payload);
            
            ProcessEnterState(stateHandler);
        }
        
        public void Put<TType>(TType instance) where TType : T
        {
            States[typeof(TType)] = instance;
        }

        public void Clear()
        { 
            if (ActualState.Value != null)
            {
                ActualState.Value.Exit();
            }
            States.Clear();
        }

        private bool TryGetHandler<TType>(out T handler) where TType : T
        {
            handler = default;
            if (!States.TryGetValue(typeof(TType), out var stateHandler) || stateHandler == null)
            {
                Debug.LogError($"No state {typeof(T)}");
                return false;
            }

            handler = stateHandler;
            return true;
        }

        private void ExitActualState()
        {
            if (ActualState.Value == null) return;
            ActualState.Value.Exit();
        }

        private void ProcessEnterState(T stateHandler)
        {
            _actualState.Value = stateHandler;
            stateHandler.Enter();
        }
    }
}
using System;
using System.Collections.Generic;
using Sheriff.GameFlow.States.ClassicGame;
using Sirenix.OdinInspector;
using UniRx;

namespace Sheriff.GameFlow
{
    public interface IAllowedActionsProvider
    {
        IReadOnlyCollection<Type> AllowedActions { get; }
    }

    public interface IAllowedActionsProviderWritable
    {
        void ApplyAllowedActions(ICollection<Type> allowedActions);
        void ClearAllowedActions();
    }

    public interface IActualStateProvider
    {
        IReadOnlyReactiveProperty<ISubState> ActualState { get; }
    }
    
    public interface IActualStateProviderWritable : IActualStateProvider
    {
        void SetState(ISubState actualState);
    }
    
    public class AllowedActionsProvider : IAllowedActionsProvider, IAllowedActionsProviderWritable
    {
        private readonly HashSet<Type> _allowedActions = new();
        
        [ShowInInspector]
        public IReadOnlyCollection<Type> AllowedActions => _allowedActions;
        
        public void ApplyAllowedActions(ICollection<Type> allowedActions)
        {
            ClearAllowedActions();
            foreach (var action in allowedActions)
            {
                _allowedActions.Add(action);
            }
        }

        public void ClearAllowedActions()
        {
            _allowedActions.Clear();
        }

    }


    public class ActualStateProviderProvider : IActualStateProvider, IActualStateProviderWritable
    {
        private readonly ReactiveProperty<ISubState> _actualState = new();

        [ShowInInspector]
        public IReadOnlyReactiveProperty<ISubState> ActualState => _actualState;


        public void SetState(ISubState actualState)
        {
            _actualState.Value = actualState;
        }
    }
}
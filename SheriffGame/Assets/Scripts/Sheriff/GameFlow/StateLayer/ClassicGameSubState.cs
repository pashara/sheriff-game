using System.Collections.Generic;
using Sheriff.GameFlow.IterationEnvironments;
using ThirdParty.StateMachine.States;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame
{
    public interface IClassicSubState<out T> : IState where T : ClassicGameState 
    {
        
    }
    
    public class ClassicGameSubState<T> : SubState<T>, IClassicSubState<T> where T : ClassicGameState
    {
        private readonly DiContainer _container;
        private readonly IterationEnvironmentFactory _environmentFactory;
        private IterationEnvironment _environment;
        private T _stateInstance;

        public ClassicGameSubState(
            DiContainer container,
            IterationEnvironmentFactory environmentFactory
            )
        {
            _container = container;
            _environmentFactory = environmentFactory;
        }

        public sealed override void Enter()
        {
            if (_environment != null)
            {
                _environment.Dispose();
                _environment = null;
            }
            _environment = CreateEnvironment();
            _environment.Container.BindInterfacesAndSelfTo<ActionHandleService>().AsSingle();
            
            OnEnter();
        }

        public sealed override void Exit()
        {
            foreach (var applyHandler in _environment.ResolveId<IEnumerable<IStateApplyable>>(_stateInstance))
                applyHandler.Apply();
            
            OnExit();
            if (_environment != null)
            {
                _environment.Dispose();
                _environment = null;
            }
        }

        protected virtual void OnEnter()
        {
            _stateInstance = _environment.Instantiate<T>();
            _stateInstance.Enter();
        }

        protected virtual void OnExit()
        {
            _stateInstance.Exit();
            _stateInstance = null;
        }

        private IterationEnvironment CreateEnvironment() => _environmentFactory.Create(_container);
    }
}
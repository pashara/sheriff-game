using ThirdParty.StateMachine.States;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame
{
    public abstract class ClassicGameState : IState
    {
        public abstract string Title { get; }
        protected DiContainer Container { get; private set; }


        public abstract void Enter();

        public abstract void Exit();
    }
}
using ThirdParty.StateMachine.States;

namespace Sheriff.GameFlow.States.ClassicGame
{
    public abstract class SubState<T> : IState
    {
        public abstract void Enter();

        public abstract void Exit();
    }
}
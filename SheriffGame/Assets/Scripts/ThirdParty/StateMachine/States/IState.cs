
namespace ThirdParty.StateMachine.States
{
    public interface IStatePayload { }
    
    public interface IPayloadableState<T>
    {
        void Configure(T payload);
    }
    
    public interface IState
    {
        void Enter();
        void Exit();
    }
    
    public interface IProcessableState
    {
        void Process();
    }

    public interface IStateMachine<T> where T : IState
    {
        void Enter<TState>() where TState : T;
        void Enter<TState, TPayload>(TPayload payload) 
            where TState : T, IPayloadableState<TPayload>
            where TPayload : IStatePayload;
    }

    public interface IStateMachineTickable<T> where T : IState
    {
        void Tick();
    }
}

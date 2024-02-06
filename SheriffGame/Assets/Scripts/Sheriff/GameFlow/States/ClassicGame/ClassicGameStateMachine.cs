using Sheriff.ECS;
using ThirdParty.StateMachine;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame
{
    public class ClassicGameStateMachine : StateMachine<ClassicGameState>
    {
        [Inject] private EcsContextProvider _ecsContextProvider;
        protected override void BeforeEnterState(ClassicGameState actualStateValue)
        {
            base.BeforeEnterState(actualStateValue);
            _ecsContextProvider.Context.game.gameIdEntity?.actualStateProviderWritable.Value.SetState(actualStateValue);
        }
        // protected override void BeforeExitState(ClassicGameState actualStateValue)
    // {
    //     base.BeforeExitState(actualStateValue);
    //     // actualStateValue.ApplyData();
    // }
    }
}
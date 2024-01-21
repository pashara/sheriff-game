using Sheriff.ECS;
using Sheriff.GameFlow.IterationEnvironments;
using Sheriff.GameFlow.States.ClassicGame.States.SetSherif;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States.SheriffCheck
{
    public class SheriffCheckState : ClassicGameState
    {
        private readonly ClassicGameController _classicGameController;
        private readonly EcsContextProvider _ecsContextProvider;
        private readonly IterationEnvironment _iterationEnvironment;

        public SheriffCheckState(
            ClassicGameController classicGameController,
            EcsContextProvider ecsContextProvider,
            IterationEnvironment iterationEnvironment)
        {
            _classicGameController = classicGameController;
            _ecsContextProvider = ecsContextProvider;
            _iterationEnvironment = iterationEnvironment;
            BindEnvironment(_iterationEnvironment.Container);
        }
        
        public override void Enter()
        {
            _classicGameController.OnReady<SetSheriffStatusState>();
        }


        protected void BindEnvironment(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<SherifSelectService>().AsSingle();
        }

        public override void Exit()
        {
        }
    }
}
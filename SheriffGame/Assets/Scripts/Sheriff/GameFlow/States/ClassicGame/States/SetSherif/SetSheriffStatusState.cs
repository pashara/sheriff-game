using System.Collections.Generic;
using Sheriff.ECS;
using Sheriff.GameFlow.IterationEnvironments;
using Sheriff.GameFlow.States.ClassicGame.States.StopState;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States.SetSherif
{
    public class SetSheriffStatusState : ClassicGameState
    {
        private readonly ClassicGameController _classicGameController;
        private readonly EcsContextProvider _ecsContextProvider;
        private readonly IterationEnvironment _iterationEnvironment;

        public SetSheriffStatusState(
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
            var gameEntity = _ecsContextProvider.Context.game.gameIdEntity;

            var actualAction = _iterationEnvironment.Instantiate<SelectSheriffAction>();
            
            var simulationResult = actualAction.Simulate(new SelectSheriffActionParam()
            {
                round = gameEntity.round.Value,
                playersQueue = gameEntity.potentialPlayersSequence.Value
            });

            actualAction.Emulate(simulationResult);
            
            _classicGameController.OnReady<SetSheriffStatusState>();
        }


        protected void BindEnvironment(DiContainer container)
        {
            container.BindInterfacesAndSelfTo<SherifSelectService>().AsSingle().WithConcreteId(this);
        }


        public override void Exit()
        {
        }
    }
}
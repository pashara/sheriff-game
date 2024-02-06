using Sheriff.ECS;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States.SetSherif
{
    public class SetSheriffStatusState : ClassicGameState
    {
        private readonly DiContainer _container;
        private readonly ClassicGameController _classicGameController;
        private readonly CommandsApplyService _commandsApplyService;
        private readonly EcsContextProvider _ecsContextProvider;

        public SetSheriffStatusState(
            DiContainer container,
            ClassicGameController classicGameController,
            CommandsApplyService commandsApplyService,
            EcsContextProvider ecsContextProvider)
        {
            _container = container;
            _classicGameController = classicGameController;
            _commandsApplyService = commandsApplyService;
            _ecsContextProvider = ecsContextProvider;
        }
        
        public override void Enter()
        {
            var gameEntity = _ecsContextProvider.Context.game.gameIdEntity;

            var actualAction = _container
                .Instantiate<SelectSheriffCommand>()
                .Calculate(new SelectSheriffCommand.Params()
                {
                    round = gameEntity.round.Value,
                    playersQueue = gameEntity.potentialPlayersSequence.Value
                });
            _commandsApplyService.Apply(actualAction);
            
            _classicGameController.OnReady<SetSheriffStatusState>();
        }
        


        public override void Exit()
        {
        }
    }
}
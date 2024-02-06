using Sheriff.ECS;
using Sheriff.GameFlow.IterationEnvironments;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States.SetSherif
{
    public class SetSheriffStatusState : ClassicGameState
    {
        private readonly ClassicGameController _classicGameController;
        private readonly CommandsApplyService _commandsApplyService;
        private readonly EcsContextProvider _ecsContextProvider;
        private readonly IterationEnvironment _iterationEnvironment;

        public SetSheriffStatusState(
            ClassicGameController classicGameController,
            CommandsApplyService commandsApplyService,
            EcsContextProvider ecsContextProvider,
            IterationEnvironment iterationEnvironment)
        {
            _classicGameController = classicGameController;
            _commandsApplyService = commandsApplyService;
            _ecsContextProvider = ecsContextProvider;
            _iterationEnvironment = iterationEnvironment;
        }
        
        public override void Enter()
        {
            var gameEntity = _ecsContextProvider.Context.game.gameIdEntity;

            var actualAction = _iterationEnvironment
                .Command<SelectSheriffCommand>()
                .Calculate(new SelectSheriffCommand.Params()
                {
                    round = gameEntity.round.Value,
                    playersQueue = gameEntity.potentialPlayersSequence.Value
                });
            _commandsApplyService.Apply(actualAction);
            
            _classicGameController.OnReady<SetSherifStatusSubState>();
        }
        


        public override void Exit()
        {
        }
    }
}
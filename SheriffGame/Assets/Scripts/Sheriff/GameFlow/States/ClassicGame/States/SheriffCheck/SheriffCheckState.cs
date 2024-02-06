using System;
using System.Collections.Generic;
using Sheriff.ECS;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States.SheriffCheck
{
    public class SheriffCheckState : ClassicGameState
    {
        private readonly ClassicGameController _classicGameController;
        private readonly EcsContextProvider _ecsContextProvider;
        private readonly DiContainer _container;
        private readonly CommandsApplyService _commandsApplyService;

        public SheriffCheckState(
            ClassicGameController classicGameController,
            EcsContextProvider ecsContextProvider,
            DiContainer container,
            CommandsApplyService commandsApplyService)
        {
            _classicGameController = classicGameController;
            _ecsContextProvider = ecsContextProvider;
            _container = container;
            _commandsApplyService = commandsApplyService;
        }
        
        public override void Enter()
        {
            foreach (var playerEntity in _ecsContextProvider.Context.player.GetEntities())
            {
                if (playerEntity.isDealer)
                {
                    var allowedActions = new AllowedActionsProvider();
                    allowedActions.ApplyAllowedActions(new List<Type>()
                    {
                        typeof(AffectGoldCommand),
                    });
                    playerEntity.isReadyForCheck = true;
                    playerEntity.ReplaceAllowedActions(allowedActions);
                }
                else if (playerEntity.isSheriff)
                {
                    var allowedActions = new AllowedActionsProvider();
                    allowedActions.ApplyAllowedActions(new List<Type>()
                    {
                        typeof(AffectGoldCommand),
                        typeof(CheckDealersCommand),
                    });
                    playerEntity.ReplaceAllowedActions(allowedActions);
                    
                }
            }
            // ResetDeclaration();
            
            // _classicGameController.OnReady<SheriffCheckState>();
        }

        private void IncRound()
        {
            var gameEntity = _ecsContextProvider.Context.game.gameIdEntity;
            gameEntity.ReplaceRound(gameEntity.round.Value + 1);
        }

        private void ResetDeclaration()
        {
            foreach (var e in _ecsContextProvider.Context.player.GetEntities())
            {
                var actualAction = _container
                    .Instantiate<ResetDeclarationCommand>()
                    .Calculate(new ResetDeclarationCommand.Params()
                    {
                        playerEntityId = e.playerId.Value
                    });
                _commandsApplyService.Apply(actualAction);
                e.isReadyForCheck = false;
            }
        }

        public override void Exit()
        {
            IncRound();
            ResetDeclaration();
        }
    }
}
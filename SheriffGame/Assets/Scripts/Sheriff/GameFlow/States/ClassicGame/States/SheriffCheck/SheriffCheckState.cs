using System;
using System.Collections.Generic;
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
        private readonly CommandsApplyService _commandsApplyService;
        private readonly IterationEnvironment _iterationEnvironment;

        public SheriffCheckState(
            ClassicGameController classicGameController,
            EcsContextProvider ecsContextProvider,
            CommandsApplyService commandsApplyService,
            IterationEnvironment iterationEnvironment)
        {
            _classicGameController = classicGameController;
            _ecsContextProvider = ecsContextProvider;
            _commandsApplyService = commandsApplyService;
            _iterationEnvironment = iterationEnvironment;
            
            
            foreach (var playerEntity in ecsContextProvider.Context.player.GetEntities())
            {
                if (playerEntity.isDealer)
                {
                    var allowedActions = new AllowedActionsProvider();
                    allowedActions.ApplyAllowedActions(new List<Type>()
                    {
                        typeof(DeclareCommand),
                        typeof(GetCardsFromDeckCommand),
                        typeof(PopCardFromBagCommand),
                        typeof(PutCardInBagCommand),
                    });
                    playerEntity.ReplaceAllowedActions(allowedActions);
                }
                else if (playerEntity.isSheriff)
                {
                    var allowedActions = new AllowedActionsProvider();
                    allowedActions.ApplyAllowedActions(new List<Type>()
                    {
                        typeof(DeclareCommand),
                        typeof(GetCardsFromDeckCommand),
                        typeof(PopCardFromBagCommand),
                        typeof(PutCardInBagCommand),
                    });
                    playerEntity.ReplaceAllowedActions(allowedActions);
                    
                }
            }
            
        }
        
        public override void Enter()
        {
            ResetDeclaration();
            
            _classicGameController.OnReady<SheriffCheckState>();
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
                var actualAction = _iterationEnvironment
                    .Command<ResetDeclarationCommand>()
                    .Calculate(new ResetDeclarationCommand.Params()
                    {
                        playerEntityId = e.playerId.Value
                    });
                _commandsApplyService.Apply(actualAction);
            }
        }

        public override void Exit()
        {
            IncRound();
            ResetDeclaration();
        }
    }
}
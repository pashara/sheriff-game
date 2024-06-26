﻿using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sheriff.ECS;
using Sheriff.GameFlow.CommandsApplier;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States.SheriffCheck
{
    public class SheriffCheckState : ClassicGameState
    {
        private readonly ClassicGameController _classicGameController;
        private readonly EcsContextProvider _ecsContextProvider;
        private readonly DiContainer _container;
        private readonly ICommandsApplyService _commandsApplyService;

        public SheriffCheckState(
            ClassicGameController classicGameController,
            EcsContextProvider ecsContextProvider,
            DiContainer container,
            ICommandsApplyService commandsApplyService)
        {
            _classicGameController = classicGameController;
            _ecsContextProvider = ecsContextProvider;
            _container = container;
            _commandsApplyService = commandsApplyService;
        }
        
        public override string Title => "Стадия Досмотра";
        
        public override void Enter()
        {
            foreach (var playerEntity in _ecsContextProvider.Context.player.GetEntities())
            {
                if (playerEntity.isDealer)
                {
                    var allowedActions = new AllowedActionsProvider();
                    allowedActions.ApplyAllowedActions(new List<Type>()
                    {
                    });
                    playerEntity.isReadyForCheck = true;
                    playerEntity.ReplaceAllowedActions(allowedActions);
                }
                else if (playerEntity.isSheriff)
                {
                    var allowedActions = new AllowedActionsProvider();
                    allowedActions.ApplyAllowedActions(new List<Type>()
                    {
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
                _commandsApplyService.Apply(actualAction).Forget();
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
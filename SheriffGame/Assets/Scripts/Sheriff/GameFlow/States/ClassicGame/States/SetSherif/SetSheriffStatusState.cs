using System.Collections.Generic;
using Sheriff.ECS;
using Sheriff.ECS.Components;
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

            HashSet<CardEntityId> cardsToRelease = new();
            foreach (var player in _ecsContextProvider.Context.player.GetEntities())
            {
                // bool isSkip = 
                // if (player.hasSheriffCheckResult)
                // {
                //     player.sheriffCheckResult is SkipCheckSherifResult;
                // }
                
                if (player.hasSelectedCards)
                {
                    foreach (var cardId in player.selectedCards.Value)
                        cardsToRelease.Add(cardId);
                    player.RemoveSelectedCards();
                }

                if (player.hasDeclareResourcesByPlayer)
                {
                    player.RemoveDeclareResourcesByPlayer();
                }

                if (player.hasSheriffCheckResult)
                {
                    player.RemoveSheriffCheckResult();
                }

                player.ReplaceCardsPopPerStep(0);
            }

            foreach (var cardEntityId in cardsToRelease)
            {
                _ecsContextProvider.Context.card.GetEntityWithCardId(cardEntityId).MarkReleased();
            }
            
            
            
            
            

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
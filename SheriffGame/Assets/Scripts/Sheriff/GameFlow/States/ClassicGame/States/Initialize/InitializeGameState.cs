using System.Collections.Generic;
using System.Linq;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameFlow.States.ClassicGame.States.SetSherif;
using Sheriff.GameFlow.States.ClassicGame.View;
using Sheriff.Rules.ClassicRules;
using ThirdParty.Randoms;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States
{
    public class InitializeGameState : ClassicGameState
    {
        private readonly IRandomService _randomService;
        private readonly EcsContextProvider _ecsContextProvider;
        private readonly ClassicGameController _classicGameController;
        private readonly GameViewController _gameViewController;
        private readonly ClassicRuleConfig _ruleConfig;
        private readonly CommandsApplyService _commandsApplyService;
        private readonly DiContainer _container;

        public InitializeGameState(
            IRandomService randomService,
            EcsContextProvider ecsContextProvider, 
            ClassicGameController classicGameController,
            GameViewController gameViewController,
            CommandsApplyService commandsApplyService,
            DiContainer container,
            ClassicRuleConfig ruleConfig)
        {
            _randomService = randomService.CreateSubService();
            _ecsContextProvider = ecsContextProvider;
            _classicGameController = classicGameController;
            _gameViewController = gameViewController;
            _commandsApplyService = commandsApplyService;
            _container = container;
            _ruleConfig = ruleConfig;
        }


        private void CreateSession(ActualStateProviderProvider actualStateProvider)
        {
            var gameEntity = _ecsContextProvider.Context.game.CreateEntity();
            gameEntity.AddGameId(new GameSessionId(8800));
            gameEntity.AddActualStateProviderWritable(actualStateProvider);
            gameEntity.ReplaceRound(0);

            var allPlayers = _ecsContextProvider.Context.player
                .GetGroup(PlayerMatcher.PlayerId).AsEnumerable().Select(x => x.playerId.Value).ToList();
            
            allPlayers.Shuffle(_randomService);
            
            gameEntity.AddPotentialPlayersSequence(allPlayers);
        }
        
        
        private void CreateCards()
        {

            List<CardsConfig> cardsQueue = new();
            
            foreach (var cardsConfig in _ruleConfig.intitalCardsConfig.cardsConfigs)
            {
                for (int i = 0; i < cardsConfig.cardsCount; i++)
                {
                    cardsQueue.Add(cardsConfig);
                }
            }
            cardsQueue.Shuffle();

            int cardId = 0;
            foreach (var cardConfig in cardsQueue)
            {
                cardId++;
                var cardEntity = _ecsContextProvider.Context.card.CreateEntity();
                cardEntity.AddResourceCategory(cardConfig.category);
                cardEntity.AddResourceType(cardConfig.resourceType);
                cardEntity.AddCardId(cardId);
                cardEntity.isInDec = true;
            }
            
        }

        private long CreatePlayer(IActualStateProvider actualStateProvider)
        {
            var entity = _ecsContextProvider.Context.player.CreateEntity();
            var id = entity.id.ID;
            entity.AddPlayerId(id);
            entity.ReplaceGoldCashCurrency(0);
            entity.AddActualStateProvider(actualStateProvider);


            return id;
        }

        public override void Enter()
        {
            var actualStateProvider = new ActualStateProviderProvider();
            CreatePlayer(actualStateProvider);
            CreatePlayer(actualStateProvider);
            CreatePlayer(actualStateProvider);
            
            CreateSession(actualStateProvider);

            CreateCards();

            GiveCardsToPlayers();
            GiveGoldToPlayers();
            
            
            _gameViewController.LinkAllPlayers();
            _gameViewController.LinkDeck();
            _classicGameController.OnReady<InitializeGameState>();
        }

        private void GiveGoldToPlayers()
        {
            foreach (var playerEntity in _ecsContextProvider.Context.player.GetEntities())
            {
                playerEntity.ReplaceGoldCashCurrency(180);
            }
        }

        private void GiveCardsToPlayers()
        {
            foreach (var playerEntity in _ecsContextProvider.Context.player.GetEntities())
            {
                var action = _container.Instantiate<GetCardsFromDeckCommand>().Calculate(new GetCardsFromDeckCommand.Params()
                {
                    cardsCount = 5,
                    playerEntityId = playerEntity.playerId.Value
                });
                _commandsApplyService.Apply(action);
            }
        }

        public override void Exit()
        {
        }
    }
}
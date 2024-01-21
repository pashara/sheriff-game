using System.Collections.Generic;
using System.Linq;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameFlow.States.ClassicGame.States.SetSherif;
using Sheriff.GameFlow.States.ClassicGame.View;
using Sheriff.Rules.ClassicRules;
using ThirdParty.Randoms;

namespace Sheriff.GameFlow.States.ClassicGame.States
{
    public class InitializeGameState : ClassicGameState
    {
        private readonly IRandomService _randomService;
        private readonly EcsContextProvider _ecsContextProvider;
        private readonly ClassicGameController _classicGameController;
        private readonly GameViewController _gameViewController;
        private readonly ClassicRuleConfig _ruleConfig;

        public InitializeGameState(
            IRandomService randomService,
            EcsContextProvider ecsContextProvider, 
            ClassicGameController classicGameController,
            GameViewController gameViewController,
            ClassicRuleConfig ruleConfig)
        {
            _randomService = randomService.CreateSubService();
            _ecsContextProvider = ecsContextProvider;
            _classicGameController = classicGameController;
            _gameViewController = gameViewController;
            _ruleConfig = ruleConfig;
        }


        private void CreateSession()
        {
            var gameEntity = _ecsContextProvider.Context.game.CreateEntity();
            gameEntity.AddGameId(new GameSessionId(8800));
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

        private long CreatePlayer()
        {
            var entity = _ecsContextProvider.Context.player.CreateEntity();
            var id = entity.id.ID;
            entity.AddPlayerId(id);


            return id;
        }

        public override void Enter()
        {
            CreatePlayer();
            CreatePlayer();
            CreatePlayer();
            
            CreateSession();

            CreateCards();
            
            
            _gameViewController.LinkAllPlayers();
            _classicGameController.OnReady<InitializeGameState>();
        }

        public override void Exit()
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Sheriff.ClientServer.Game;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameFlow.CommandsApplier;
using Sheriff.GameFlow.GameStatistics;
using Sheriff.GameResources;
using Sheriff.Rules.ClassicRules;
using ThirdParty.Randoms;
using ThirdParty.StateMachine.States;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.States
{
    public class InitializeGamePayload : IStatePayload
    {
        public int PlayersCount = 0;
        public Action BeforeSync;
    }
    public class InitializeGameState : ClassicGameState, IPayloadableState<InitializeGamePayload>
    {
        private readonly IRandomService _randomService;
        private readonly EcsContextProvider _ecsContextProvider;
        private readonly ClassicGameController _classicGameController;
        private readonly ClassicRuleConfig _ruleConfig;
        private readonly ICommandsApplyService _commandsApplyService;
        private readonly DiContainer _container;
        private readonly LinkWithVisualService _linkWithVisualService;
        private readonly IPunSender _punManager;
        private int _playersCount;
        private Action _beforeSync;

        public InitializeGameState(
            IRandomService randomService,
            EcsContextProvider ecsContextProvider, 
            ClassicGameController classicGameController,
            ICommandsApplyService commandsApplyService,
            DiContainer container,
            LinkWithVisualService linkWithVisualService,
            IPunSender punManager,
            ClassicRuleConfig ruleConfig)
        {
            _randomService = randomService.CreateSubService();
            _ecsContextProvider = ecsContextProvider;
            _classicGameController = classicGameController;
            _commandsApplyService = commandsApplyService;
            _container = container;
            _linkWithVisualService = linkWithVisualService;
            _punManager = punManager;
            _ruleConfig = ruleConfig;
        }


        private void CreateSession(IActualStateProviderWritable actualStateProvider)
        {
            var gameEntity = _ecsContextProvider.Context.game.CreateEntity();
            gameEntity.AddGameId(new GameSessionId(8800));
            gameEntity.AddActualStateProviderWritable(actualStateProvider);
            gameEntity.AddAllowedToDeclareGameResources(new List<GameResourceType>()
            {
                GameResourceType.Apple,
                GameResourceType.Cheese,
                GameResourceType.Bread,
                GameResourceType.Chicken,
                // GameResourceType.Pepper,
                // GameResourceType.Honeydew,
                // GameResourceType.Silk,
                // GameResourceType.Сrossbow,
            });
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
                cardEntity.PutInDec();
            }
            
        }

        private long CreatePlayer(IActualStateProvider actualStateProvider)
        {
            var entity = _ecsContextProvider.Context.player.CreateEntity();
            var id = entity.id.ID;
            entity.AddPlayerId(id);
            entity.ReplacePlayerStatistics(new PlayerStatistics());
            entity.ReplaceNickname(entity.playerId.Value.ToString());
            entity.ReplaceGoldCashCurrency(0);
            entity.ReplaceMaxCardsPopPerStep(5);
            entity.AddActualStateProvider(actualStateProvider);


            return id;
        }

        public override string Title => "Initialize";

        public override void Enter()
        {
            var actualStateProvider = new ActualStateProviderProvider();
            for (int i = 0; i < _playersCount; i++)
            {
                CreatePlayer(actualStateProvider);
            }

            // CreatePlayer(actualStateProvider);
            CreateSession(actualStateProvider);
            CreateCards();
            GiveGoldToPlayers();

            _beforeSync?.Invoke();
            _punManager.SendInitialGameState();
            
            GiveCardsToPlayers();

            _classicGameController.OnReady<InitializeGameState>();
        }

        private void GiveGoldToPlayers()
        {
            foreach (var playerEntity in _ecsContextProvider.Context.player.GetEntities())
            {
                playerEntity.ReplaceGoldCashCurrency(180);
            }
        }

        private async void GiveCardsToPlayers()
        {
            foreach (var playerEntity in _ecsContextProvider.Context.player.GetEntities())
            {
                var action = _container.Instantiate<GetCardsFromDeckCommand>().Calculate(new GetCardsFromDeckCommand.Params()
                {
                    cardsCount = 6,
                    playerEntityId = playerEntity.playerId.Value,
                    ignoreLimits = true
                });
                await _commandsApplyService.Apply(action);
            }
        }

        public override void Exit()
        {
        }

        public void Configure(InitializeGamePayload payload)
        {
            _playersCount = payload.PlayersCount;
            _beforeSync = payload.BeforeSync;
        }
    }
}
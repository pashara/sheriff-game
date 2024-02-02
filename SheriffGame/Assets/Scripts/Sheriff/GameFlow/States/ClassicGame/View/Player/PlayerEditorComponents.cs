using System;
using System.Collections.Generic;
using System.Linq;
using Sheriff.ECS;
using Sheriff.ECS.Components;
using Sheriff.GameResources;
using Sirenix.OdinInspector;
using UniRx;
using Zenject;

namespace Sheriff.GameFlow.States.ClassicGame.View.Player
{
    public class PlayerEditorComponents : SerializedMonoBehaviour
    {
        [Serializable]
        private class CardInfo
        {
            public long cardId;
            public GameResourceType resourceType;
            public GameResourceCategory resourceCategory;

            public CardInfo(CardEntity card)
            {
                cardId = card.cardId.Value.EntityID;
                resourceType = card.resourceType.Value;
                resourceCategory = card.resourceCategory.Value;
            }
        }
        [Inject] private CommandsApplyService _commandsApplyService;
        [Inject] private DiContainer _container;
        [Inject] private EcsContextProvider _ecsContextProvider;
        [Inject] private SheriffCheckHandler _sheriffCheckHandler;
        private PlayerEntity _playerEntity;


        [ShowInInspector]
        [ReadOnly]
        private int Gold
        {
            get;
            set;
        }
        
        [ShowInInspector]
        [ReadOnly]
        private bool IsSheriff
        {
            get;
            set;
        }
        
        [ShowInInspector]
        [ReadOnly]
        private UserActionsList ActualState
        {
            get;
            set;
        }
        
        [ShowInInspector]
        [ReadOnly]
        private IAllowedActionsProvider AllowedActions
        {
            get;
            set;
        }
        
        

        [ShowInInspector]
        [ReadOnly]
        private List<CardInfo> Cards
        {
            get;
        } = new();


        [ShowInInspector]
        [ReadOnly]
        private List<CardInfo> CardsInBag
        {
            get;
        } = new();

        [ShowInInspector]
        [ReadOnly]
        private ProductsDeclaration Declaration
        {
            get;
            set;
        }
        

        public void Link(PlayerEntity playerEntity)
        {
            _playerEntity = playerEntity;
            playerEntity.OnSheriff()
                .Select(x => x != null)
                .Subscribe(x => IsSheriff = x)
                .AddTo(this);

            LinkState(playerEntity);
            LinkActionsList(playerEntity);
            LinkCards(playerEntity);
            LinkDeclaration(playerEntity);
        }


        private void LinkActionsList(PlayerEntity playerEntity)
        {
            playerEntity.OnAllowedActions()
                .Subscribe(x =>
                {
                    AllowedActions = x?.Value ?? new AllowedActionsProvider();
                })
                .AddTo(this);
        }

        private void LinkState(PlayerEntity playerEntity)
        {
            playerEntity.OnActualStateProvider()
                .Subscribe(x => ActualState = x?.Value)
                .AddTo(this);
            
            playerEntity.OnGoldCashCurrency()
                .Subscribe(x => Gold = x?.Value ?? 0)
                .AddTo(this);
        }

        private void LinkDeclaration(PlayerEntity playerEntity)
        {
            playerEntity.OnDeclareResourcesByPlayer()
                .Subscribe(x => Declaration = x?.Value)
                .AddTo(this);
        }


        private void LinkCards(PlayerEntity playerEntity)
        {
            foreach (var card in _ecsContextProvider.Context.card.GetEntities())
            {
                card.OnCardOwner().Subscribe(x =>
                {
                    var element = Cards.FirstOrDefault(_ => _.cardId == card.cardId.Value);
                    
                    if (x == null && element != null)
                        Cards.Remove(element);
                    else if (x != null && x.Value == playerEntity.playerId.Value)
                        Cards.Add(new CardInfo(card));
                });
            }

            playerEntity.OnSelectedCards().Subscribe(x =>
            {
                CardsInBag.Clear();
                if (x == null)
                    return;
                
                foreach (var cardEntityId in x.Value)
                {
                    CardsInBag.Add(new CardInfo(_ecsContextProvider.Context.card.GetEntityWithCardId(cardEntityId)));
                }
            });
        }


        private bool CanDeclare => AllowedActions.AllowedActions.Contains(typeof(DeclareCommand));
        [ShowIf(nameof(CanDeclare))]
        [Button]
        private void Declare(List<ProductDeclaration> declarations)
        {
            var action = _container.Instantiate<DeclareCommand>().Calculate(new DeclareCommand.Params()
            {
                playerEntityId = _playerEntity.playerId.Value,
                declarations = new ProductsDeclaration(declarations)
            });
            _commandsApplyService.Apply(action);
        }

        private bool CanGetCard => AllowedActions.AllowedActions.Contains(typeof(GetCardsFromDeckCommand));
        [ShowIf(nameof(CanGetCard))]
        [Button]
        private void GetCard()
        {
            var action = _container.Instantiate<GetCardsFromDeckCommand>().Calculate(new GetCardsFromDeckCommand.Params()
            {
                cardsCount = 1,
                playerEntityId = _playerEntity.playerId.Value
            });
            _commandsApplyService.Apply(action);
        }

        private bool CanPutCardInBag => AllowedActions.AllowedActions.Contains(typeof(PutCardInBagCommand));
        [ShowIf(nameof(CanPutCardInBag))]
        [Button]
        private void PutCardInBag(long cardId)
        {
            var action = _container.Instantiate<PutCardInBagCommand>().Calculate(new PutCardInBagCommand.Params()
            {
                playerEntityId = _playerEntity.playerId.Value,
                cardEntityId = cardId
            });
            _commandsApplyService.Apply(action);
        }

        private bool CanPutCardFromBag => AllowedActions.AllowedActions.Contains(typeof(PopCardFromBagCommand));
        [ShowIf(nameof(CanPutCardFromBag))]
        [Button]
        private void PopCardFromBag(long cardId)
        {
            var action = _container.Instantiate<PopCardFromBagCommand>().Calculate(new PopCardFromBagCommand.Params()
            {
                playerEntityId = _playerEntity.playerId.Value,
                cardEntityId = cardId
            });
            _commandsApplyService.Apply(action);
        }
        

        private bool CanCheckAsSheriff => AllowedActions.AllowedActions.Contains(typeof(CheckDealersCommand));
        [ShowIf(nameof(CanCheckAsSheriff))]
        [Button]
        private void CheckAsSherif(long playerId)
        {
            var check = _sheriffCheckHandler.Check(_playerEntity.playerId.Value, new PlayerEntityId(playerId));
            
            var action = _container.Instantiate<CheckDealersCommand>().Calculate(new CheckDealersCommand.Params()
            {
                CheckResult = check
            });
            _commandsApplyService.Apply(action);
        }

        [ShowIf(nameof(CanCheckAsSheriff))]
        [Button]
        private void SkipCharacterAsSherif(long playerId)
        {
            var check = _sheriffCheckHandler.Skip(_playerEntity.playerId.Value, new PlayerEntityId(playerId));
            
            var action = _container.Instantiate<CheckDealersCommand>().Calculate(new CheckDealersCommand.Params()
            {
                CheckResult = check
            });
            _commandsApplyService.Apply(action);
        }
        
    }
}